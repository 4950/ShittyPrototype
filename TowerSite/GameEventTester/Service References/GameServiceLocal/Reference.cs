//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Original file name:
// Generation date: 3/5/2014 8:27:53 PM
namespace GameEventTester.GameServiceLocal
{
    
    /// <summary>
    /// There are no comments for Container in the schema.
    /// </summary>
    public partial class Container : global::System.Data.Services.Client.DataServiceContext
    {
        /// <summary>
        /// Initialize a new Container object.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public Container(global::System.Uri serviceRoot) : 
                base(serviceRoot, global::System.Data.Services.Common.DataServiceProtocolVersion.V3)
        {
            this.ResolveName = new global::System.Func<global::System.Type, string>(this.ResolveNameFromType);
            this.ResolveType = new global::System.Func<string, global::System.Type>(this.ResolveTypeFromName);
            this.OnContextCreated();
            this.Format.LoadServiceModel = GeneratedEdmModel.GetInstance;
        }
        partial void OnContextCreated();
        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected global::System.Type ResolveTypeFromName(string typeName)
        {
            global::System.Type resolvedType = this.DefaultResolveType(typeName, "TowerSite.Models", "GameEventTester.GameServiceLocal");
            if ((resolvedType != null))
            {
                return resolvedType;
            }
            return null;
        }
        /// <summary>
        /// Since the namespace configured for this service reference
        /// in Visual Studio is different from the one indicated in the
        /// server schema, use type-mappers to map between the two.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected string ResolveNameFromType(global::System.Type clientType)
        {
            if (clientType.Namespace.Equals("GameEventTester.GameServiceLocal", global::System.StringComparison.Ordinal))
            {
                return string.Concat("TowerSite.Models.", clientType.Name);
            }
            return null;
        }
        /// <summary>
        /// There are no comments for GameEvent in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceQuery<GameEventModel> GameEvent
        {
            get
            {
                if ((this._GameEvent == null))
                {
                    this._GameEvent = base.CreateQuery<GameEventModel>("GameEvent");
                }
                return this._GameEvent;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceQuery<GameEventModel> _GameEvent;
        /// <summary>
        /// There are no comments for GameSession in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.Data.Services.Client.DataServiceQuery<GameSession> GameSession
        {
            get
            {
                if ((this._GameSession == null))
                {
                    this._GameSession = base.CreateQuery<GameSession>("GameSession");
                }
                return this._GameSession;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.Data.Services.Client.DataServiceQuery<GameSession> _GameSession;
        /// <summary>
        /// There are no comments for GameEvent in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public void AddToGameEvent(GameEventModel gameEventModel)
        {
            base.AddObject("GameEvent", gameEventModel);
        }
        /// <summary>
        /// There are no comments for GameSession in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public void AddToGameSession(GameSession gameSession)
        {
            base.AddObject("GameSession", gameSession);
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private abstract class GeneratedEdmModel
        {
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            private static global::Microsoft.Data.Edm.IEdmModel ParsedModel = LoadModelFromString();
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            private const string ModelPart0 = @"<edmx:Edmx Version=""1.0"" xmlns:edmx=""http://schemas.microsoft.com/ado/2007/06/edmx""><edmx:DataServices m:DataServiceVersion=""3.0"" m:MaxDataServiceVersion=""3.0"" xmlns:m=""http://schemas.microsoft.com/ado/2007/08/dataservices/metadata""><Schema Namespace=""TowerSite.Models"" xmlns=""http://schemas.microsoft.com/ado/2009/11/edm""><EntityType Name=""GameEventModel""><Key><PropertyRef Name=""ID"" /></Key><Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" /><Property Name=""SessionId"" Type=""Edm.Int32"" Nullable=""false"" /><Property Name=""Timestamp"" Type=""Edm.DateTime"" Nullable=""false"" /><Property Name=""Type"" Type=""Edm.String"" /><Property Name=""Data"" Type=""Edm.String"" /></EntityType><EntityType Name=""GameSession""><Key><PropertyRef Name=""ID"" /></Key><Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" /><Property Name=""UserID"" Type=""Edm.String"" /><Property Name=""SessionID"" Type=""Edm.Int32"" Nullable=""false"" /></EntityType></Schema><Schema Namespace=""Default"" xmlns=""http://schemas.microsoft.com/ado/2009/11/edm""><EntityContainer Name=""Container"" m:IsDefaultEntityContainer=""true""><EntitySet Name=""GameEvent"" EntityType=""TowerSite.Models.GameEventModel"" /><EntitySet Name=""GameSession"" EntityType=""TowerSite.Models.GameSession"" /></EntityContainer></Schema></edmx:DataServices></edmx:Edmx>";
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            private static string GetConcatenatedEdmxString()
            {
                return string.Concat(ModelPart0);
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            public static global::Microsoft.Data.Edm.IEdmModel GetInstance()
            {
                return ParsedModel;
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            private static global::Microsoft.Data.Edm.IEdmModel LoadModelFromString()
            {
                string edmxToParse = GetConcatenatedEdmxString();
                global::System.Xml.XmlReader reader = CreateXmlReader(edmxToParse);
                try
                {
                    return global::Microsoft.Data.Edm.Csdl.EdmxReader.Parse(reader);
                }
                finally
                {
                    ((global::System.IDisposable)(reader)).Dispose();
                }
            }
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
            private static global::System.Xml.XmlReader CreateXmlReader(string edmxToParse)
            {
                return global::System.Xml.XmlReader.Create(new global::System.IO.StringReader(edmxToParse));
            }
        }
    }
    /// <summary>
    /// There are no comments for TowerSite.Models.GameEventModel in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("GameEvent")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("ID")]
    public partial class GameEventModel : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new GameEventModel object.
        /// </summary>
        /// <param name="ID">Initial value of ID.</param>
        /// <param name="sessionId">Initial value of SessionId.</param>
        /// <param name="timestamp">Initial value of Timestamp.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static GameEventModel CreateGameEventModel(int ID, int sessionId, global::System.DateTime timestamp)
        {
            GameEventModel gameEventModel = new GameEventModel();
            gameEventModel.ID = ID;
            gameEventModel.SessionId = sessionId;
            gameEventModel.Timestamp = timestamp;
            return gameEventModel;
        }
        /// <summary>
        /// There are no comments for Property ID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this._ID = value;
                this.OnIDChanged();
                this.OnPropertyChanged("ID");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _ID;
        partial void OnIDChanging(int value);
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for Property SessionId in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int SessionId
        {
            get
            {
                return this._SessionId;
            }
            set
            {
                this.OnSessionIdChanging(value);
                this._SessionId = value;
                this.OnSessionIdChanged();
                this.OnPropertyChanged("SessionId");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _SessionId;
        partial void OnSessionIdChanging(int value);
        partial void OnSessionIdChanged();
        /// <summary>
        /// There are no comments for Property Timestamp in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public global::System.DateTime Timestamp
        {
            get
            {
                return this._Timestamp;
            }
            set
            {
                this.OnTimestampChanging(value);
                this._Timestamp = value;
                this.OnTimestampChanged();
                this.OnPropertyChanged("Timestamp");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private global::System.DateTime _Timestamp;
        partial void OnTimestampChanging(global::System.DateTime value);
        partial void OnTimestampChanged();
        /// <summary>
        /// There are no comments for Property Type in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this.OnTypeChanging(value);
                this._Type = value;
                this.OnTypeChanged();
                this.OnPropertyChanged("Type");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Type;
        partial void OnTypeChanging(string value);
        partial void OnTypeChanged();
        /// <summary>
        /// There are no comments for Property Data in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string Data
        {
            get
            {
                return this._Data;
            }
            set
            {
                this.OnDataChanging(value);
                this._Data = value;
                this.OnDataChanged();
                this.OnPropertyChanged("Data");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _Data;
        partial void OnDataChanging(string value);
        partial void OnDataChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
    /// <summary>
    /// There are no comments for TowerSite.Models.GameSession in the schema.
    /// </summary>
    /// <KeyProperties>
    /// ID
    /// </KeyProperties>
    [global::System.Data.Services.Common.EntitySetAttribute("GameSession")]
    [global::System.Data.Services.Common.DataServiceKeyAttribute("ID")]
    public partial class GameSession : global::System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new GameSession object.
        /// </summary>
        /// <param name="ID">Initial value of ID.</param>
        /// <param name="sessionID">Initial value of SessionID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static GameSession CreateGameSession(int ID, int sessionID)
        {
            GameSession gameSession = new GameSession();
            gameSession.ID = ID;
            gameSession.SessionID = sessionID;
            return gameSession;
        }
        /// <summary>
        /// There are no comments for Property ID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this.OnIDChanging(value);
                this._ID = value;
                this.OnIDChanged();
                this.OnPropertyChanged("ID");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _ID;
        partial void OnIDChanging(int value);
        partial void OnIDChanged();
        /// <summary>
        /// There are no comments for Property UserID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public string UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                this.OnUserIDChanging(value);
                this._UserID = value;
                this.OnUserIDChanged();
                this.OnPropertyChanged("UserID");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private string _UserID;
        partial void OnUserIDChanging(string value);
        partial void OnUserIDChanged();
        /// <summary>
        /// There are no comments for Property SessionID in the schema.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public int SessionID
        {
            get
            {
                return this._SessionID;
            }
            set
            {
                this.OnSessionIDChanging(value);
                this._SessionID = value;
                this.OnSessionIDChanged();
                this.OnPropertyChanged("SessionID");
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        private int _SessionID;
        partial void OnSessionIDChanging(int value);
        partial void OnSessionIDChanged();
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        protected virtual void OnPropertyChanged(string property)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new global::System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }
    }
}
