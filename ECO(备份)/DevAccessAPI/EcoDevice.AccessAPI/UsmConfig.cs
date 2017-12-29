using System;
namespace EcoDevice.AccessAPI
{
	public class UsmConfig
	{
		private string _engineId = string.Empty;
		private string _securityName = string.Empty;
		private Authentication _authenticationType;
		private Privacy _privacyType;
		private string _authenticationSecret;
		private string _privacySecret;
		public string EngineId
		{
			get
			{
				return this._engineId;
			}
		}
		public string SecurityName
		{
			get
			{
				return this._securityName;
			}
		}
		public Authentication Authentication
		{
			get
			{
				return this._authenticationType;
			}
		}
		public Privacy Privacy
		{
			get
			{
				return this._privacyType;
			}
		}
		public string AuthenticationSecret
		{
			get
			{
				return this._authenticationSecret;
			}
		}
		public string PrivacySecret
		{
			get
			{
				return this._privacySecret;
			}
		}
		public UsmConfig()
		{
			this._engineId = string.Empty;
			this._securityName = string.Empty;
			this._authenticationType = Authentication.None;
			this._authenticationSecret = string.Empty;
			this._privacyType = Privacy.None;
			this._privacySecret = string.Empty;
		}
		public UsmConfig(string engineId, string securityName) : this()
		{
			this._engineId = engineId;
			this._securityName = securityName;
		}
		public UsmConfig(string engineId, string securityName, Authentication authDigest, string authSecret) : this(engineId, securityName)
		{
			this._authenticationType = authDigest;
			this._authenticationSecret = authSecret;
		}
		public UsmConfig(string engineId, string securityName, Authentication authDigest, string authSecret, Privacy privType, string privSecret) : this(engineId, securityName, authDigest, authSecret)
		{
			this._privacyType = privType;
			this._privacySecret = privSecret;
		}
		public bool IsMatch(string engineId, string securityname)
		{
			return engineId != null && securityname != null && (this._engineId != null && this._engineId.Equals(engineId) && this._securityName != null && this._securityName.Equals(securityname));
		}
		public bool IsMatch(string securityname)
		{
			return string.IsNullOrEmpty(this._engineId) && this._securityName.Equals(securityname);
		}
	}
}
