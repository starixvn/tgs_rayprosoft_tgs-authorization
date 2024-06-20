import type { CreateRole, Role } from "../models/role";
import { Observable, concatMap, map } from "rxjs";
import type { HttpClient } from '@angular/common/http';
import { SystemFunction } from "../models/system-function";

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

    public setRolePriority(roleId: string, priority: number): Observable<void | null> {
        return this._http.patch<void>(
            `/api/role/${roleId}`,
            {
                priority
            },
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

    public getFunctionsBySystem(system: string): Observable<SystemFunction> {
        return this._http
            .get<SystemFunction[]>(`/api/function/${system}`, { observe: 'response' })
            .pipe(
                concatMap(res => res.body ?? [])
            );
    }

    public createFunction(system: string, name: string): Observable<string | null> {
        return this._http
            .put<string>(
                `/api/function`,
                {
                    system,
                    name
                },
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

    public renameFunction(id: string, newName: string): Observable<void | null> {
        return this._http
            .patch<void>(
                `/api/function/${id}`,
                JSON.stringify(newName),
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

    public deleteFunction(id: string): Observable<void | null> {
        return this._http
            .delete<void>(
                `/api/function/${id}`,
                {
                    observe: 'response'
                })
            .pipe(
                map(res => res.body)
            );
    }
}
