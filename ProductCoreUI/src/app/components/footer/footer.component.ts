import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  currentDate: Date = new Date();

subscribe() {
throw new Error('Method not implemented.');
}
getYear(): number {
  return this.currentDate.getFullYear();
}

}
