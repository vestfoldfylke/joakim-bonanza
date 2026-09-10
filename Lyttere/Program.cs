using balleA;
using balleB;

// Variant A

var _sesh = new Session();
Console.WriteLine(_sesh.IsLoaded);

new ManagerDirect(_sesh).DeleteActive();
Console.WriteLine(_sesh.IsLoaded);
