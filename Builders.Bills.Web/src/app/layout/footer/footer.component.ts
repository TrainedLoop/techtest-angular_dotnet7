import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  footerText = "© 2023 Daniel Porto";
  footerLink = "https://www.linkedin.com/in/daniel-porto/";
}
