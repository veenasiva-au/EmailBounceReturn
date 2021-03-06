﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmailBounceBack.DataLayer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="EmailBounceBack")]
	public partial class EmailBounceBackDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertMailBox(MailBox instance);
    partial void UpdateMailBox(MailBox instance);
    partial void DeleteMailBox(MailBox instance);
    partial void InsertDocuments(Documents instance);
    partial void UpdateDocuments(Documents instance);
    partial void DeleteDocuments(Documents instance);
    partial void InsertSetting(Setting instance);
    partial void UpdateSetting(Setting instance);
    partial void DeleteSetting(Setting instance);
    partial void InsertResendEmail(ResendEmail instance);
    partial void UpdateResendEmail(ResendEmail instance);
    partial void DeleteResendEmail(ResendEmail instance);
    #endregion
		
		public EmailBounceBackDataContext() : 
				base(global::EmailBounceBack.Properties.Settings.Default.EmailBounceBackConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public EmailBounceBackDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EmailBounceBackDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EmailBounceBackDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public EmailBounceBackDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<MailBox> MailBoxes
		{
			get
			{
				return this.GetTable<MailBox>();
			}
		}
		
		public System.Data.Linq.Table<Documents> Documents
		{
			get
			{
				return this.GetTable<Documents>();
			}
		}
		
		public System.Data.Linq.Table<Setting> Settings
		{
			get
			{
				return this.GetTable<Setting>();
			}
		}
		
		public System.Data.Linq.Table<ResendEmail> ResendEmails
		{
			get
			{
				return this.GetTable<ResendEmail>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MailBoxes")]
	public partial class MailBox : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _MailboxGUID;
		
		private string _ProfileObject;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMailboxGUIDChanging(System.Guid value);
    partial void OnMailboxGUIDChanged();
    partial void OnProfileObjectChanging(string value);
    partial void OnProfileObjectChanged();
    #endregion
		
		public MailBox()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MailboxGUID", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid MailboxGUID
		{
			get
			{
				return this._MailboxGUID;
			}
			set
			{
				if ((this._MailboxGUID != value))
				{
					this.OnMailboxGUIDChanging(value);
					this.SendPropertyChanging();
					this._MailboxGUID = value;
					this.SendPropertyChanged("MailboxGUID");
					this.OnMailboxGUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ProfileObject", DbType="NVarChar(MAX)")]
		public string ProfileObject
		{
			get
			{
				return this._ProfileObject;
			}
			set
			{
				if ((this._ProfileObject != value))
				{
					this.OnProfileObjectChanging(value);
					this.SendPropertyChanging();
					this._ProfileObject = value;
					this.SendPropertyChanged("ProfileObject");
					this.OnProfileObjectChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Documents")]
	public partial class Documents : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _DocumentID;
		
		private string _DocumentPath;
		
		private string _Subject;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDocumentIDChanging(int value);
    partial void OnDocumentIDChanged();
    partial void OnDocumentPathChanging(string value);
    partial void OnDocumentPathChanged();
    partial void OnSubjectChanging(string value);
    partial void OnSubjectChanged();
    #endregion
		
		public Documents()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DocumentID", DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true)]
		public int DocumentID
		{
			get
			{
				return this._DocumentID;
			}
			set
			{
				if ((this._DocumentID != value))
				{
					this.OnDocumentIDChanging(value);
					this.SendPropertyChanging();
					this._DocumentID = value;
					this.SendPropertyChanged("DocumentID");
					this.OnDocumentIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="DripfeedPath", Storage="_DocumentPath", DbType="varchar(255)")]
		public string DocumentPath
		{
			get
			{
				return this._DocumentPath;
			}
			set
			{
				if ((this._DocumentPath != value))
				{
					this.OnDocumentPathChanging(value);
					this.SendPropertyChanging();
					this._DocumentPath = value;
					this.SendPropertyChanged("DocumentPath");
					this.OnDocumentPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="EmailSubject", Storage="_Subject", DbType="varchar(255)", CanBeNull=false, IsPrimaryKey=true)]
		public string Subject
		{
			get
			{
				return this._Subject;
			}
			set
			{
				if ((this._Subject != value))
				{
					this.OnSubjectChanging(value);
					this.SendPropertyChanging();
					this._Subject = value;
					this.SendPropertyChanged("Subject");
					this.OnSubjectChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Settings")]
	public partial class Setting : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Name;
		
		private string _Value;
		
		private string _Comments;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnValueChanging(string value);
    partial void OnValueChanged();
    partial void OnCommentsChanging(string value);
    partial void OnCommentsChanged();
    #endregion
		
		public Setting()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Value", DbType="VarChar(1024)")]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if ((this._Value != value))
				{
					this.OnValueChanging(value);
					this.SendPropertyChanging();
					this._Value = value;
					this.SendPropertyChanged("Value");
					this.OnValueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Comments", DbType="VarChar(1024)")]
		public string Comments
		{
			get
			{
				return this._Comments;
			}
			set
			{
				if ((this._Comments != value))
				{
					this.OnCommentsChanging(value);
					this.SendPropertyChanging();
					this._Comments = value;
					this.SendPropertyChanged("Comments");
					this.OnCommentsChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ResendEmails")]
	public partial class ResendEmail : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _EmailID;
		
		private System.Nullable<System.Guid> _MailboxGUID;
		
		private string _MessageID;
		
		private System.Nullable<int> _DocumentID;
		
		private string _Subject;
		
		private string _OriginalDocumentPath;
		
		private string _OriginalEmailSubject;
		
		private string _ResendSubject;
		
		private System.Nullable<int> _RetryCount;
		
		private System.Xml.Linq.XElement _Errors;
		
		private string _Status;
		
		private System.Nullable<bool> _InProgress;
		
		private System.Nullable<System.DateTime> _TimeStamp;
		
		private System.Nullable<System.DateTime> _StartTime;
		
		private System.Nullable<System.DateTime> _EndTime;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnEmailIDChanging(int value);
    partial void OnEmailIDChanged();
    partial void OnMailboxGUIDChanging(System.Nullable<System.Guid> value);
    partial void OnMailboxGUIDChanged();
    partial void OnMessageIDChanging(string value);
    partial void OnMessageIDChanged();
    partial void OnDocumentIDChanging(System.Nullable<int> value);
    partial void OnDocumentIDChanged();
    partial void OnSubjectChanging(string value);
    partial void OnSubjectChanged();
    partial void OnOriginalDocumentPathChanging(string value);
    partial void OnOriginalDocumentPathChanged();
    partial void OnOriginalEmailSubjectChanging(string value);
    partial void OnOriginalEmailSubjectChanged();
    partial void OnResendSubjectChanging(string value);
    partial void OnResendSubjectChanged();
    partial void OnRetryCountChanging(System.Nullable<int> value);
    partial void OnRetryCountChanged();
    partial void OnErrorsChanging(System.Xml.Linq.XElement value);
    partial void OnErrorsChanged();
    partial void OnStatusChanging(string value);
    partial void OnStatusChanged();
    partial void OnInProgressChanging(System.Nullable<bool> value);
    partial void OnInProgressChanged();
    partial void OnTimeStampChanging(System.Nullable<System.DateTime> value);
    partial void OnTimeStampChanged();
    partial void OnStartTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnStartTimeChanged();
    partial void OnEndTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnEndTimeChanged();
    #endregion
		
		public ResendEmail()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EmailID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int EmailID
		{
			get
			{
				return this._EmailID;
			}
			set
			{
				if ((this._EmailID != value))
				{
					this.OnEmailIDChanging(value);
					this.SendPropertyChanging();
					this._EmailID = value;
					this.SendPropertyChanged("EmailID");
					this.OnEmailIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MailboxGUID", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> MailboxGUID
		{
			get
			{
				return this._MailboxGUID;
			}
			set
			{
				if ((this._MailboxGUID != value))
				{
					this.OnMailboxGUIDChanging(value);
					this.SendPropertyChanging();
					this._MailboxGUID = value;
					this.SendPropertyChanged("MailboxGUID");
					this.OnMailboxGUIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MessageID", DbType="NVarChar(255)")]
		public string MessageID
		{
			get
			{
				return this._MessageID;
			}
			set
			{
				if ((this._MessageID != value))
				{
					this.OnMessageIDChanging(value);
					this.SendPropertyChanging();
					this._MessageID = value;
					this.SendPropertyChanged("MessageID");
					this.OnMessageIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DocumentID", DbType="Int")]
		public System.Nullable<int> DocumentID
		{
			get
			{
				return this._DocumentID;
			}
			set
			{
				if ((this._DocumentID != value))
				{
					this.OnDocumentIDChanging(value);
					this.SendPropertyChanging();
					this._DocumentID = value;
					this.SendPropertyChanged("DocumentID");
					this.OnDocumentIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Subject", DbType="NVarChar(255)")]
		public string Subject
		{
			get
			{
				return this._Subject;
			}
			set
			{
				if ((this._Subject != value))
				{
					this.OnSubjectChanging(value);
					this.SendPropertyChanging();
					this._Subject = value;
					this.SendPropertyChanged("Subject");
					this.OnSubjectChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OriginalDocumentPath", DbType="NVarChar(255)")]
		public string OriginalDocumentPath
		{
			get
			{
				return this._OriginalDocumentPath;
			}
			set
			{
				if ((this._OriginalDocumentPath != value))
				{
					this.OnOriginalDocumentPathChanging(value);
					this.SendPropertyChanging();
					this._OriginalDocumentPath = value;
					this.SendPropertyChanged("OriginalDocumentPath");
					this.OnOriginalDocumentPathChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_OriginalEmailSubject", DbType="NVarChar(255)")]
		public string OriginalEmailSubject
		{
			get
			{
				return this._OriginalEmailSubject;
			}
			set
			{
				if ((this._OriginalEmailSubject != value))
				{
					this.OnOriginalEmailSubjectChanging(value);
					this.SendPropertyChanging();
					this._OriginalEmailSubject = value;
					this.SendPropertyChanged("OriginalEmailSubject");
					this.OnOriginalEmailSubjectChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ResendSubject", DbType="NVarChar(255)")]
		public string ResendSubject
		{
			get
			{
				return this._ResendSubject;
			}
			set
			{
				if ((this._ResendSubject != value))
				{
					this.OnResendSubjectChanging(value);
					this.SendPropertyChanging();
					this._ResendSubject = value;
					this.SendPropertyChanged("ResendSubject");
					this.OnResendSubjectChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RetryCount", DbType="Int")]
		public System.Nullable<int> RetryCount
		{
			get
			{
				return this._RetryCount;
			}
			set
			{
				if ((this._RetryCount != value))
				{
					this.OnRetryCountChanging(value);
					this.SendPropertyChanging();
					this._RetryCount = value;
					this.SendPropertyChanged("RetryCount");
					this.OnRetryCountChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Errors", DbType="Xml", UpdateCheck=UpdateCheck.Never)]
		public System.Xml.Linq.XElement Errors
		{
			get
			{
				return this._Errors;
			}
			set
			{
				if ((this._Errors != value))
				{
					this.OnErrorsChanging(value);
					this.SendPropertyChanging();
					this._Errors = value;
					this.SendPropertyChanged("Errors");
					this.OnErrorsChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="NVarChar(50)")]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InProgress", DbType="Bit")]
		public System.Nullable<bool> InProgress
		{
			get
			{
				return this._InProgress;
			}
			set
			{
				if ((this._InProgress != value))
				{
					this.OnInProgressChanging(value);
					this.SendPropertyChanging();
					this._InProgress = value;
					this.SendPropertyChanged("InProgress");
					this.OnInProgressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TimeStamp", DbType="DateTime")]
		public System.Nullable<System.DateTime> TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				if ((this._TimeStamp != value))
				{
					this.OnTimeStampChanging(value);
					this.SendPropertyChanging();
					this._TimeStamp = value;
					this.SendPropertyChanged("TimeStamp");
					this.OnTimeStampChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> StartTime
		{
			get
			{
				return this._StartTime;
			}
			set
			{
				if ((this._StartTime != value))
				{
					this.OnStartTimeChanging(value);
					this.SendPropertyChanging();
					this._StartTime = value;
					this.SendPropertyChanged("StartTime");
					this.OnStartTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndTime", DbType="DateTime")]
		public System.Nullable<System.DateTime> EndTime
		{
			get
			{
				return this._EndTime;
			}
			set
			{
				if ((this._EndTime != value))
				{
					this.OnEndTimeChanging(value);
					this.SendPropertyChanging();
					this._EndTime = value;
					this.SendPropertyChanged("EndTime");
					this.OnEndTimeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
