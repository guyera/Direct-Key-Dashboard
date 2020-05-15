
using System;

namespace DirectKeyDashboard.Testing {
    public class UnitTest {
        public string Name {get; set;}
        public Action Action {get; set;}
        
        public UnitTest(Action action, string name) {
            Action = action;
            Name = name;
        }
    }
}
