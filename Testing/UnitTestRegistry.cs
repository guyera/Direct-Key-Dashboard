
using System;
using System.Collections.Generic;

namespace DirectKeyDashboard.Testing {
    public class UnitTestRegistry {
        private static UnitTestRegistry _singleton = null;
        private IList<UnitTest> _unitTests = new List<UnitTest>();

        private const int _paddingLength = 9;
        private const int _errorDashesLength = 10;

        private UnitTestRegistry() {}
        
        public static UnitTestRegistry GetDefault() {
            if (_singleton == null) {
                _singleton = new UnitTestRegistry();
            }
            return _singleton;
        }

        public void Register(UnitTest unitTest) {
            _unitTests.Add(unitTest);
        }

        private void PrintHeader(UnitTest unitTest) {
            var dashesLength = _paddingLength * 2 + 2 + unitTest.Name.Length;
            var dashes = "";
            for (var i = 0; i < dashesLength; i++) {
                dashes += '-';
            }
            var namePadding = "";
            for (var i = 0; i < _paddingLength; i++) {
                namePadding += ' ';
            }
            Console.WriteLine(dashes);
            Console.WriteLine($"|{namePadding}{unitTest.Name}{namePadding}|");
            Console.WriteLine($"{dashes}\n");
        }

        private void PrintError(UnitTest unitTest, AssertionException e) {
            Console.ForegroundColor = ConsoleColor.Red;
            var dashes = "";
            for (var i = 0; i < _errorDashesLength; i++) {
                dashes += '-';
            }
            Console.WriteLine($"|{dashes} ERROR: Unit test \"{unitTest.Name}\" FAILED {dashes}|\n");
            Console.WriteLine($"{e.StackTrace}\n");
            Console.ResetColor();
        }

        public void RunTests() {
            Console.WriteLine("Running unit tests...");
            bool failed = false;
            foreach (var unitTest in _unitTests) {
                PrintHeader(unitTest);
                try {
                    unitTest.Action();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Unit test \"{unitTest.Name}\" passed.\n");
                    Console.ResetColor();
                } catch(AssertionException e) {
                    failed = true;
                    PrintError(unitTest, e);
                }
            }

            if (failed) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: FAILED one or more unit tests. Exiting...\n");
                Console.ResetColor();
                Environment.Exit(-1);
            }
        }
    }
}