import { Observable } from 'rxjs';
import fetch from 'node-fetch';
import AbortController from 'abort-controller';

export function fromFetch(input, init) {
    return new Observable(subscriber => {
        const controller = new AbortController();
        const signal = controller.signal;
        let outerSignalHandler;
        let abortable = true;
        let unsubscribed = false;

        if (init) {
            // If a signal is provided, just have it teardown. It's a cancellation token, basically.
            if (init.signal) {
                outerSignalHandler = () => {
                    if (!signal.aborted) {
                        controller.abort();
                    }
                };
                init.signal.addEventListener('abort', outerSignalHandler);
            }
            init.signal = signal;
        } else {
            init = { signal };
        }

        fetch(input, init).then(response => {
            abortable = false;
            subscriber.next(response);
            subscriber.complete();
        }).catch(err => {
            abortable = false;
            if (!unsubscribed) {
                // Only forward the error if it wasn't an abort.
                subscriber.error(err);
            }
        });

        return () => {
            unsubscribed = true;
            if (abortable) {
                controller.abort();
            }
        };
    });
}