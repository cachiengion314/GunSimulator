using System;

[Serializable]
public struct LobbyData
{
  public ParameterLobbyDatas parameters;
}

[Serializable]
public struct ParameterLobbyDatas
{
  public AdmobData Admob;
  public UpdateConfigData update_config;
  public ConfigGameData config_game;
}

//
[Serializable]
public struct AdmobData
{
  public AdmobDeviceData ios;
  public AdmobDeviceData android;
}

[Serializable]
public struct AdmobDeviceData
{
  public AdmobTypeData admob_AOA;
}

[Serializable]
public struct AdmobTypeData
{
  public bool isShow;
  public string value;
}

//
[Serializable]
public struct ConfigGameData
{
  public int energy_refill_time;
  public int max_energy;
}

//
[Serializable]
public struct UpdateConfigData
{
  public UpdateConfigDeviceData ios;
  public UpdateConfigDeviceData android;
}

[Serializable]
public struct UpdateConfigDeviceData
{
  public string version;
  public string link_app;
  public int reward;
}

