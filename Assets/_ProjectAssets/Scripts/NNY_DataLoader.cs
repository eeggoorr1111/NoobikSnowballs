using Narratore.Data;
using Narratore.Localization;
using System.Collections.Generic;
using UnityEngine;


namespace Narratore
{

    public class NNY_DataLoader : DataLoader
    {
        public NNY_DataLoader(Mode mode, DataProviders providers) : base(mode, providers)
        {
            _int = new GPPlayerIntDataSource();
            _float = new GPPlayerFloatDataSource();
            _bool = new GPPlayerBoolDataSource(); 
            _gaLvlsOrder = new GARemoteConfigLvlsOrderSource();

            _remoteFloat = new GARemoteConfigFloatSource[providers.RemoteFloat.Count];
            for (int i = 0; i < providers.RemoteFloat.Count; i++)
                _remoteFloat[i] = new GARemoteConfigFloatSource();

            _remoteInt = new GARemoteConfigIntSource[providers.RemoteInt.Count];
            for (int i = 0; i < providers.RemoteInt.Count; i++)
                _remoteInt[i] = new GARemoteConfigIntSource();

            _remoteBool = new GARemoteConfigBoolSource[providers.RemoteBool.Count];
            for (int i = 0; i < providers.RemoteBool.Count; i++)
                _remoteBool[i] = new GARemoteConfigBoolSource();

            _providers = providers;
        }

        private readonly GPPlayerIntDataSource _int;
        private readonly GPPlayerBoolDataSource _bool;
        private readonly GPPlayerFloatDataSource _float;
        private readonly GARemoteConfigFloatSource[] _remoteFloat;
        private readonly GARemoteConfigIntSource[] _remoteInt;
        private readonly GARemoteConfigBoolSource[] _remoteBool;
        private readonly GARemoteConfigLvlsOrderSource _gaLvlsOrder;
        private readonly DataProviders _providers;


        protected override void SetSource(DataProvider<int> provider) =>
            provider.Load(_int);

        protected override void SetSource(DataProvider<float> provider) =>
            provider.Load(_float);

        protected override void SetSource(DataProvider<bool> provider) =>
            provider.Load(_bool);


        protected override void LoadImpl()
        {
            for (int i = 0; i < _providers.RemoteInt.Count; i++)
                Load(_providers.RemoteInt[i], _prefsIntSource, _remoteInt[i]);

            for (int i = 0; i < _providers.RemoteFloat.Count; i++)
                Load(_providers.RemoteFloat[i], _prefsFloatSource, _remoteFloat[i]);

            for (int i = 0; i < _providers.RemoteBool.Count; i++)
                Load(_providers.RemoteBool[i], _prefsBoolSource, _remoteBool[i]);
        }


        protected override IDataSource<DeviceType> CustomDeviceSource() =>
            new GPDeviceTypeDataSource();

        protected override IDataSource<LangKey> CustomLangSource() =>
            new GPLangDataSource();

        protected override IDataSource<IReadOnlyDictionary<int, int>> CustomLvlsOrder() =>
            _gaLvlsOrder;


        public override void Dispose()
        {
            for (int i = 0; i < _remoteInt.Length; i++)
                _remoteInt[i].Dispose();

            for (int i = 0; i < _remoteFloat.Length; i++)
                _remoteFloat[i].Dispose();

            for (int i = 0; i < _remoteBool.Length; i++)
                _remoteBool[i].Dispose();

            _gaLvlsOrder.Dispose();
        }
    }
}

