export class Notification {
    public title: string;
    public description: string;
    public text: string;
    public data: string;
    public notifications: Array<Notification>;
  
    constructor() {
      this.notifications = new Array<Notification>();
    }
  }
  