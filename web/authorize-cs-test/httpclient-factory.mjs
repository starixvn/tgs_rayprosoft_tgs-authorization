import { HttpClient, HttpEventType, HttpErrorResponse, HttpHeaders, HttpResponse } from '@angular/common/http';
import { FETCH_TOKEN } from '@ngify/http/fetch';
import { catchError, concatMap, map, from, of, switchMap, throwError, filter, tap } from 'rxjs';
import { fromFetch } from './fetch.mjs';
import http from 'http';

class NamedPipeFetchBackend {
    #channelName;

    constructor(channelName) {
        this.#channelName = channelName;
    }

    handle(request) {
        let requestUrl = `${request.url}`;

        if (requestUrl.startsWith('/'))
            requestUrl = requestUrl.slice(1);

        if (requestUrl.startsWith('api/'))
            requestUrl = requestUrl.slice(4);

        let modifyRequest = request.clone({
            url: `http://localhost/${requestUrl}`
        });

        let agent = new http.Agent({
            path: modifyRequest.url,
            socketPath: `\\\\.\\pipe\\${this.#channelName}`
        });

        if (modifyRequest.reportProgress) {
            throw Error('Fetch API does not currently support report progress');
        }

        let url;
        let status;
        let statusText;
        let headers;

        return of({ type: HttpEventType.Sent }).pipe(
            concatMap(() => fromFetch(modifyRequest.urlWithParams, {
                method: modifyRequest.method,
                headers: modifyRequest.headers.keys().reduce((headers, name) => (
                    headers[name] = modifyRequest.headers.getAll(name).join(','),
                    headers
                ), {}),
                body: modifyRequest.serializeBody(),
                ...modifyRequest.context.get(FETCH_TOKEN),
                agent
            })),
            switchMap(response => {
                url = response.url;
                status = response.status;
                statusText = response.statusText;
                headers = new HttpHeaders();

                response.headers.forEach((value, key) => headers.set(key, value));

                switch (modifyRequest.responseType) {
                    case 'arraybuffer':
                        return from(response.arrayBuffer());

                    case 'blob':
                        return from(response.blob());

                    case 'json':
                        return from(response.text()).pipe(
                            map(t => typeof (t) == 'string' && t.length > 0
                                ? JSON.parse(t)
                                : undefined)
                        );

                    case 'text':
                        return from(response.text());
                }
            }),
            map(body => new HttpResponse({
                body,
                headers,
                status,
                statusText,
                url,
            })),
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    return throwError(() => error);
                }

                return throwError(() => new HttpErrorResponse({
                    url,
                    error: error,
                    status,
                    statusText,
                    headers,
                }));
            })
        );
    }
}

class NormalFetchBackend {
    handle(request) {
        let requestUrl = `${request.url}`;

        if (requestUrl.startsWith('/'))
            requestUrl = requestUrl.slice(1);

        if (requestUrl.startsWith('api/'))
            requestUrl = requestUrl.slice(4);

        let modifyRequest = request.clone({
            url: `http://localhost:5014/${requestUrl}`
        });

        if (modifyRequest.reportProgress) {
            throw Error('Fetch API does not currently support report progress');
        }

        let url;
        let status;
        let statusText;
        let headers;

        return of({ type: HttpEventType.Sent }).pipe(
            concatMap(() => fromFetch(modifyRequest.urlWithParams, {
                method: modifyRequest.method,
                headers: modifyRequest.headers.keys().reduce((headers, name) => (
                    headers[name] = modifyRequest.headers.getAll(name).join(','),
                    headers
                ), {}),
                body: modifyRequest.serializeBody(),
                ...modifyRequest.context.get(FETCH_TOKEN),
            })),
            switchMap(response => {
                url = response.url;
                status = response.status;
                statusText = response.statusText;
                headers = new HttpHeaders();

                response.headers.forEach((value, key) => headers.set(key, value));

                switch (modifyRequest.responseType) {
                    case 'arraybuffer':
                        return from(response.arrayBuffer());

                    case 'blob':
                        return from(response.blob());

                    case 'json':
                        return from(response.text()).pipe(
                            map(t => typeof (t) == 'string' && t.length > 0
                                ? JSON.parse(t)
                                : undefined)
                        );

                    case 'text':
                        return from(response.text());
                }
            }),
            map(body => new HttpResponse({
                body,
                headers,
                status,
                statusText,
                url,
            })),
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    return throwError(() => error);
                }

                return throwError(() => new HttpErrorResponse({
                    url,
                    error: error,
                    status,
                    statusText,
                    headers,
                }));
            })
        );
    }
}

export function buildHttpClient(channelName, isWindows) {
    if (isWindows)
        return new HttpClient(
            new NamedPipeFetchBackend(channelName));
    else
        return new HttpClient(new HttpFetchBackend());
}

export function buildFetchClient() {
    return new HttpClient(new NormalFetchBackend());
}