import '@angular/compiler';
import { AuthorizeClient } from 'authorize-client';
import { toArray, firstValueFrom, tap, catchError, concatMap, map, of, from, switchMap, throwError } from 'rxjs';
import { HttpClient, FetchBackend } from '@angular/common/http';
import { fromFetch } from './fetch.mjs';
import { buildHttpClient, buildFetchClient } from './httpclient-factory.mjs';

let client = new AuthorizeClient(buildFetchClient());
let roleId = 'f725fb35-4010-4dfe-87c3-e05f7ba67034';

await firstValueFrom(client.deleteRole(roleId));
