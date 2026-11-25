namespace Brimborium.Angular.Repository;

/*
public class Event { }
*/

public sealed record EventUpdate<T>(T Item);