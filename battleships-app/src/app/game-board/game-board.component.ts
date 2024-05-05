import { Component, OnInit } from '@angular/core';
import { GameService } from '../game.service';
import {
  trigger,
  transition,
  state,
  style,
  animate,
} from '@angular/animations';

type AnimState = 'start' | 'end';
@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css'],
  animations: [
    trigger('anim', [
      state('start', style({ background: 'red' })),
      state('end', style({ background: 'none' })),
      transition('start => end', [animate(1000)]),
    ]),
  ],
})
export class GameBoardComponent implements OnInit {
  grid: string[][] = [];
  feedback: string = '';
  ships: any[] = [];
  disableShots: boolean = false;
  public animState: AnimState = 'end';

  constructor(private gameService: GameService) {}

  ngOnInit(): void {
    this.initializeGrid();
    this.fetchShips();
  }

  initializeGrid() {
    // Initialize the grid
    for (let i = 0; i < 10; i++) {
      this.grid[i] = Array(10).fill('');
    }
    ``;
  }

  fetchShips() {
    // Get the list of ships
    this.gameService.getShips().subscribe(
      (response: any) => {
        if (response.ships) {
          this.ships = [];
          this.ships = response.ships;
        }
      },
      (error) => {
        console.error('Error firing shot:', error);
      }
    );
  }

  fireShot(row: number, col: number) {
    this.launchAnimation();
    // Send shot coordinates
    this.gameService.fireShot(row, col).subscribe(
      (response: any) => {
        if (response.hit) {
          this.grid[row][col] = 'X';
          this.feedback = 'Hit!!!';
        } else {
          this.grid[row][col] = 'O';
          this.feedback = 'Shot Missed!!!';
        }
        if (response.sunk) {
          this.feedback += ' You sunk a ship!!!';
        }
        // Update the list of remaining ships
        if (response.ships) {
          this.ships = response.ships;
          if (this.ships.length === 0) {
            this.feedback =
              'Game Over!!' + '\n' + 'You have sunk all the ships!!!';
            this.disableShots = true;
          }
        }
      },
      (error) => {
        console.error('Error firing shot:', error);
        this.feedback = 'Error firing shot. Please try again.';
      }
    );
  }

  resetGame() {
    this.gameService.resetGame().subscribe(
      (response: any) => {
        if (response.ships) {
          console.log(response);
          this.initializeGrid();
          this.ships.length = 0;
          this.ships = response.ships;
          this.disableShots = false;
          this.feedback = 'Game Restarted Successfully!!!';
        }
      },
      (error) => {
        console.error('Error restarting game:', error);
      }
    );
  }

  // Return CSS class based on cell content
  getCellClass(cell: string): string {
    if (cell === 'X') {
      return 'hit';
    } else if (cell === 'O') {
      return 'miss';
    } else {
      return '';
    }
  }

  getShipImage(shipType: string): string {
    if (shipType === 'Battleship') {
      return 'assets/battleship.png';
    } else if (shipType === 'Destroyer') {
      return 'assets/destroyership.png';
    } else {
      return '';
    }
  }

  public launchAnimation() {
    this.animState = 'start';
    setTimeout(() => {
      this.animState = 'end';
    }, 10);
  }
}
