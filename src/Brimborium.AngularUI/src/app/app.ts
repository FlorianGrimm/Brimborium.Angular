import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RootUrl } from './service/root-url';

@Component({
  imports: [RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  constructor() {
    const baseUrl = inject(RootUrl).baseUrl;
    console.log("baseUrl", baseUrl);
  }
  protected readonly title = signal('Brimborium.AngularUI');
}
