
export class Constants {

  static readonly Languajes = ["en-US", "es-CO"];
  static Keys = class {
    static readonly Token = "token";
    static readonly MeterID = "meterID";
    static readonly BeginDate = "begin";
    static readonly EndDate = "end";
    static readonly Bearer = "Bearer ";
    static readonly InletOutletKey = "i";
    static readonly BalanceKey = "b";
    static readonly Context = "context";
    static readonly PeriodObject = "PeriodObject";
    static readonly NodeObject = "NodeObject";
    static readonly Sum = "s";
    static readonly ModeScreen = "Mode";
    static readonly Lang = "lang";
    static readonly DefaultLang = "en-US";
    static readonly SelectedNodeId = "selectedNode";
    static SelectedNodeOpen = "selectedNodeOpen";
    static readonly UserName = "username";
    static readonly Loggedin = "loggedin";
    static readonly idUser = "idUser";
    static readonly Kart = "Kart";
    static readonly idCategoria = "idCategoria";



  };

  static Errors = class {
    static readonly NoContent = 204;
    static readonly NotFound = 404;
  };

}
