import { Injectable } from "@angular/core";
import * as Crypto from "crypto-js";
import { BaseApiService } from "../base-api.service";

@Injectable({
  providedIn: "root"
})
export class SecurityService {
  constructor(private baseAPI: BaseApiService) {}

  userAuthentication(username, password) {
    const body = { email: username, password: this.transformPassword(password)};
    const vUrl = `${this.baseAPI.webserviceurl}/Account/Login`;
    return this.baseAPI.http
      .post(vUrl, body, { observe: "response", responseType: "json" })
      .catch(this.baseAPI.errorHandler);
  }

  public transformPassword(pPassword: string):string{
    const key: string = Crypto.enc.Hex.parse(
      "92AE31A79FEEB2A377ACBCB7D9A4A5C1"
    );
    const iv: string = Crypto.enc.Hex.parse("000102030405060708090a0b0c0d0e0f");
    const encrypted = Crypto.AES.encrypt(pPassword, key, {
      keySize: 128 / 8,
      iv: iv,
      mode: Crypto.mode.CBC,
      padding: Crypto.pad.Pkcs7
    });
    return encrypted.toString();
  }
}
