import type { CreateRole, Role } from "../models/role";
import { Observable, concatMap, map } from "rxjs";
import type { HttpClient } from '@angular/common/http';

export class AuthorizeClient {
    constructor(private _http: HttpClient) {
    }

    public getRolesBySystem(system: string): Observable<Role> {
        return this._http
            .get<Role[]>(`/api/role/${system}`, { observe: 'response' })
            .pipe(
                concatMap(res => res.body ?? [])
            );
    }

    public createRole(system: string, model: CreateRole): Observable<string | null> {
        return this._http.put<string>(
            `/api/role/${system}`,
            model,
            {
                headers: {
                    'Content-Type': 'application/json'
                },
                observe: 'response',
                responseType: 'json'
            })
            .pipe(
                map(res => res.body)
            );
    }

    public deleteRole(roleId: string): Observable<void | null> {
        return this._http.delete<void>(
            `/api/role/${roleId}`,
            {
                observe: 'response',
                responseType: 'json'
            })
            .pipe(
                map(res => res.body)
            );
    }
}
