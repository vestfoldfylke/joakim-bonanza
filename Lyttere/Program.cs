using balleA;
using balleB;

// Variant A

var _sesh = new Session();
Console.WriteLine(_sesh.IsLoaded);

new ManagerDirect(_sesh).DeleteActive();
Console.WriteLine(_sesh.IsLoaded);

// Variant B
var sl = new SessionListener();
var sl2 = new SessionListener();

sl.InitRunAction();
sl2.InitRunAction();

