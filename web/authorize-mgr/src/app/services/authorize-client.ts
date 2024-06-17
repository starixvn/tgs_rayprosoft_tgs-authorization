import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AuthorizeClient as LibAuthorizeClient } from 'authorize-client';
export { Role, CreateRole } from 'authorize-client';

@Injectable({
    providedIn: 'root'
})
export class AuthorizeClient extends LibAuthorizeClient {
    constructor(@Inject(HttpClient) _http: HttpClient) {
        super(_http);
    }
}
