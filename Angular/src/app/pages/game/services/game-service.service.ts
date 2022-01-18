import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {GamePublicDto} from "../game-dto";
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GameService {
  uriPublicGame = environment.apiUrl + "/PublicGame"

  constructor(private http: HttpClient) { }

  getAllGame(): Observable<GamePublicDto[]>{
    return this.http.get<GamePublicDto[]>(this.uriPublicGame+"/all").pipe(map(data => data))
  }
}
