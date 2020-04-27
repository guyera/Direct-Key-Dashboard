namespace DirectKeyDashboard.Views.Charting {
    public class CreateCustomViewModel {
        public static ViewType[] ViewTypes {get; set;} = {
            new ViewType{
                Name = "Bar Chart",
                Id = ViewType.TypeId.Bar
            },
            new ViewType{
                Name = "Grouped Bar Chart",
                Id = ViewType.TypeId.GroupedBar
            }
        };

        public class ViewType {
            public string Name {get; set;}
            public TypeId Id {get; set;}

            public enum TypeId {
                Bar,
                GroupedBar
            }
        }
    }
}