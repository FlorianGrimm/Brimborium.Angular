import { HttpHandlerFn, HttpRequest } from "@angular/common/http";

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {

  // Inject the current `AuthService` and use it to get an authentication token:
  //const authToken = inject(AuthService).getAuthToken();

  // Clone the request to add the authentication header.
  // const newReq = req.clone({
  //   headers: req.headers.append('X-Authentication-Token', authToken),
  // });

  const newReq = req.clone({});
  return next(newReq);
}
