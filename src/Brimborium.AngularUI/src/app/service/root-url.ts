import { Service } from '@angular/core';

@Service()
export class RootUrl {
  private _baseUrl: string | undefined = undefined;

  constructor() { }

  public get baseUrl(): string {
    if (null == this._baseUrl) {
      const baseUrl = this.getBaseUrl();
      if (null == baseUrl) {
        throw new Error("rootUrl is undefined.");
      } else {
        this._baseUrl = baseUrl;
      }
    }
    return this._baseUrl;
  }

  public set baseUrl(value: string) {
    this._baseUrl = value;
  }

  public getBaseUrl(): (string | undefined) {
    const location = window.location;
    if (null == location) {
      return undefined;
    } else {
      const { protocol, hostname, port } = location;
      if ((protocol === "https:" && (!port || port == "443"))
        || (protocol === "http:" && (!port || port == "80"))
        || (!port)
      ) {
        return `${protocol}//${hostname}`;
      } else {
        return `${protocol}//${hostname}:${port}`;
      }
    }
  }
}
