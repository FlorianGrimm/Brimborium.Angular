import { Component, signal } from '@angular/core';
import {form, FormField, required} from '@angular/forms/signals';

import { NavigationRoot } from '@src/app/component/navigation/navigation-root/navigation-root';

import type { Project } from '@src/generated/Brimborium.Angular.API';

@Component({
  imports: [NavigationRoot, FormField],
  selector: 'app-project-rest-index',
  styleUrl: './project-rest-index.scss',
  templateUrl: './project-rest-index.html',
})
export class ProjectRestIndex {
  projectModel = signal<Project>({
    ProjectId: '',
    Name: '',
  });
  projectForm = form(
    this.projectModel,
    (schemaPath) => {
      required(schemaPath.Name, {message: 'is required'});
    });

  onSubmit(event: Event) {
    event.preventDefault();
    // Perform login logic here
    const projectModel = this.projectModel();
    console.log('projectModel:', projectModel);
  }
}
