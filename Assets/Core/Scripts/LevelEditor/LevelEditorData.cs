// some thing about data stuffs
using System;
using NUnit.Framework;

public enum ColorType
{
  Red,
  Yellow,
  Blue,
  Green,
  Purple,
  Pink,
  Coca,
  Cyan
}

[Serializable]
public struct CupDataEditor
{
  public ColorType Color;
  public int CapacityCup;
}

[Serializable]
public struct BottleDataEditor
{
  public ColorType Color;
  public int CapacityBottle;
}

[Serializable]
public struct TrayDataEditor
{
  public ColorType Color;
}

[Serializable]
public struct CupData
{
  public int ColorIndex;
  public int CupCapacity;
  public int PosIndex;
}

[Serializable]
public struct BottleDatas
{
  public BottleData[] bottleDatas;
  public int PosIndex;
}

[Serializable]
public struct BottleData
{
  public int ColorIndex;
  public int BottleCapacity;
}

[Serializable]
public struct TrayData
{
  public int Color;
}
