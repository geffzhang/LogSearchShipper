﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace LogSearchShipper.Core.ConfigurationSections
{
	public class FileWatchElement : ConfigurationElement, IWatchElement
	{
		private FieldCollection _fieldCollection;

		[ConfigurationProperty("files", IsKey = true, IsRequired = true)]
		public String Files
		{
			get { return (String)this["files"]; }
			set { this["files"] = value; }
		}

		[ConfigurationProperty("type", IsRequired = true)]
		public String Type
		{
			get { return (String)this["type"]; }
			set { this["type"] = value; }
		}

		[ConfigurationProperty("readFromLast", IsRequired = false, DefaultValue = true)]
		public bool ReadFromLast
		{
			get { return (bool)this["readFromLast"]; }
			set { this["readFromLast"] = value; }
		}

		[ConfigurationProperty("customNxLogConfig")]
		public CustomNxlogConfig CustomNxlogConfig
		{
			get { return (CustomNxlogConfig)this["customNxLogConfig"]; }
			set { this["customNxLogConfig"] = value; }
		}

		[ConfigurationProperty("closeWhenIdle", IsRequired = false, DefaultValue = true)]
		public bool CloseWhenIdle
		{
			get { return (bool)this["closeWhenIdle"]; }
			set { this["closeWhenIdle"] = value; }
		}

		[ConfigurationProperty("sourceTailer", IsRequired = false, DefaultValue = TailerType.Normal)]
		public TailerType SourceTailer
		{
			get { return (TailerType)this["sourceTailer"]; }
			set { this["sourceTailer"] = value; }
		}

		[ConfigurationProperty("multilineRule", IsRequired = false, DefaultValue = MultilineRuleType.multiline_default)]
		public MultilineRuleType MultilineRule
		{
			get { return (MultilineRuleType)this["multilineRule"]; }
			set { this["multilineRule"] = value; }
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public FieldCollection Fields
		{
			get
			{
				if (_fieldCollection == null)
				{
					_fieldCollection = (FieldCollection)base[""];
				}
				return _fieldCollection;
			}
			set { _fieldCollection = value; }
		}

		public string Key
		{
			get { return Files; }
		}

		public override string ToString()
		{
			return string.Format("{0}", Key);
		}
	}

	public class CustomNxlogConfig : ConfigurationElement
	{
		private string _value;

		public string Value
		{
			get { return _value; }
			set { _value = (value != null) ? value.Trim() : null; }
		}

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			Value = (string)reader.ReadElementContentAs(typeof(string), null);
		}
	}

	public enum TailerType { Normal, MT }

	public enum MultilineRuleType { multiline_default, multiline_ci_log4net }
}
