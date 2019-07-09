//Developed by Pavel Kravtsov.
namespace Core.EventSystem
{
    [System.Serializable]
    public class GameEventArgs : System.EventArgs
    {
        public string type;
        public string str;
        public int? intParam;
        public float? floatParam;
        public bool boolParam;
        public object objectParam;
        public System.Action okAction;
        public System.Action cancelAction;

        public GameEventArgs(string type, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.okAction = okAction; this.cancelAction = cancelAction; }
            public GameEventArgs(string type, string str, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, int? intParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.intParam = intParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, float? floatParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.boolParam = boolParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.objectParam = objectParam; this.okAction = okAction; this.cancelAction = cancelAction; }

        public GameEventArgs(string type, string str, int? intParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, float? floatParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.boolParam = boolParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.objectParam = objectParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, int? intParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.intParam = intParam; this.boolParam = boolParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, int? intParam, float? floatParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.intParam = intParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, int? intParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.intParam = intParam; this.objectParam = objectParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, float? floatParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.boolParam = boolParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, float? floatParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.objectParam = objectParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }

        public GameEventArgs(string type, string str, int? intParam, float? floatParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, int? intParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.boolParam = boolParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, int? intParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.objectParam = objectParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, float? floatParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.boolParam = boolParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, float? floatParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.objectParam = objectParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, bool boolParam, int? intParam, float? floatParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.boolParam = boolParam; this.intParam = intParam; this.floatParam = floatParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, bool boolParam, int? intParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.boolParam = boolParam; this.intParam = intParam; this.objectParam = objectParam;  this.okAction = okAction; this.cancelAction = cancelAction; }

        public GameEventArgs(string type, string str, int? intParam, float? floatParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.floatParam = floatParam; this.boolParam = boolParam; this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, int? intParam, float? floatParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.floatParam = floatParam; this.objectParam = objectParam;  this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, int? intParam, object objectParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.objectParam = objectParam; this.boolParam = boolParam;  this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, string str, object objectParam, float? floatParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.objectParam = objectParam; this.floatParam = floatParam; this.boolParam = boolParam;  this.okAction = okAction; this.cancelAction = cancelAction; }
        public GameEventArgs(string type, object objectParam, int? intParam, float? floatParam, bool boolParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.objectParam = objectParam; this.intParam = intParam; this.floatParam = floatParam; this.boolParam = boolParam;  this.okAction = okAction; this.cancelAction = cancelAction; }

        public GameEventArgs(string type, string str, int? intParam, float? floatParam, bool boolParam, object objectParam, System.Action okAction = null, System.Action cancelAction = null) { this.type = type; this.str = str; this.intParam = intParam; this.floatParam = floatParam; this.boolParam = boolParam; this.objectParam = objectParam;  this.okAction = okAction; this.cancelAction = cancelAction; }
    }
}