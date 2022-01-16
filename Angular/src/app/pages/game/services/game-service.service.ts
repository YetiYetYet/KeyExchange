import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {map, Observable} from "rxjs";
import {GamePublicDto} from "../game-dto";

@Injectable({
  providedIn: 'root'
})
export class GameService {
  uriPublicGame = "https://localhost:7096/PublicGame"

  constructor(private http: HttpClient) { }

  getAllGame(): Observable<GamePublicDto[]>{
    return this.http.get<GamePublicDto[]>(this.uriPublicGame+"/all").pipe(map(data => data))
  }
}
