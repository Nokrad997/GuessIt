import CryptoJS from "crypto-js";

const encryptionKey = CryptoJS.enc.Base64.parse("9vUJYhLFI5hXscLjyyxKPb6sVrRWOT6PWTf98VD3Ac8=");
const iv = CryptoJS.enc.Base64.parse("L6zZzpL0RxHohq4vcBxdzQ==");

export const encryptData = (data: any) => {
    const plaintext = typeof data === "string" ? data : JSON.stringify(data);
    const encrypted = CryptoJS.AES.encrypt(plaintext, encryptionKey, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7,
    });
    console.log(encrypted.toString())
    return encrypted.toString();
};

export const decryptData = (encryptedData : any) => {
    const bytes = CryptoJS.AES.decrypt(encryptedData, encryptionKey, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7,
    });
    return JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
};
