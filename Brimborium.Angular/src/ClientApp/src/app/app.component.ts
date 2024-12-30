import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import cfg from './cfg.json';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'ClientApp';

  ngOnInit(): void {
    console.log(cfg);
    this.title = cfg.Greetings;
  }

}
