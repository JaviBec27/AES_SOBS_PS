import {
  FormControl
} from "@angular/forms";
import { ElementRef } from "@angular/core";

export function ValidateSizeImage(
    pElement: ElementRef,
    pWidth: number,
    pHeight: number
  ) {
    return (formControl: FormControl) => {
      if (pElement.nativeElement.files && pElement.nativeElement.files[0]) {
        const reader = new FileReader();
        //// Read file as data url
        reader.readAsDataURL(pElement.nativeElement.files[0]);
  
        reader.onload = data => {
          //// Called once readAsDataURL is completed
          const urlImage = data.target["result"];
          const vImg = new Image();
          vImg.onload = function (e) {
            if (vImg.width <= pWidth && vImg.height <= pHeight) {
              formControl.setErrors(null);
            } else {
              formControl.setErrors({ isValidsize: true });
            }
          };
          vImg.src = urlImage;
        };
      }
    };
  }
  