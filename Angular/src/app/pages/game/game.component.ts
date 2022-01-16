import { Component, OnInit } from '@angular/core';
import {GameService} from "./services/game-service.service";
import {GamePublicDto} from "./game-dto";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {

  games:GamePublicDto[] = []
  filter:string = "";
  error: any;
  steamIconUri:string = "https://jolstatic.fr/www/captures/796/6/121816.png"
  blizzardIconUri:string = "https://icon-library.com/images/battle-net-icon/battle-net-icon-9.jpg"

  constructor(public gameService: GameService ) { }

  ngOnInit(): void {
    this.gameService.getAllGame().subscribe({
        next: data => {
          this.games = data;
        }
    })
  }

}
