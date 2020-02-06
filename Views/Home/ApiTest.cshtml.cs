namespace DirectKeyDashboard.Views.Home {
    public class ApiTestViewModel {
        public bool Online {get; set;}
        public string Response {get; set;}

        public ApiTestViewModel(bool online, string response) {
            this.Online = online;
            this.Response = response;
        }
    }
}