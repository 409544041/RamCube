using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using Alias_rifegrt_ObjectsGameplayData = E_ObjectsGameplayData;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_Objects")]
public partial class M_Objects : BGEntityGo
{
	public override BGMetaEntity MetaConstraint
	{
		get
		{
			return MetaDefault;
		}
	}
	private static BansheeGz.BGDatabase.BGMetaRow _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaRow MetaDefault
	{
		get
		{
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(4871657982410721797UL,750137968299760546UL));
			return _metaDefault;
		}
	}
	public static BansheeGz.BGDatabase.BGRepoEvents Events
	{
		get
		{
			return BGRepo.I.Events;
		}
	}
	public System.String f_name
	{
		get
		{
			return _f_name[Entity.Index];
		}
		set
		{
			_f_name[Entity.Index] = value;
		}
	}
	public System.String f_ObjectTitle
	{
		get
		{
			return _f_ObjectTitle[Entity.Index];
		}
		set
		{
			_f_ObjectTitle[Entity.Index] = value;
		}
	}
	public System.String f_ObjectSubText
	{
		get
		{
			return _f_ObjectSubText[Entity.Index];
		}
		set
		{
			_f_ObjectSubText[Entity.Index] = value;
		}
	}
	public UnityEngine.GameObject f_Prefab
	{
		get
		{
			return _f_Prefab[Entity.Index];
		}
	}
	public E_Segments f_Owner
	{
		get
		{
			return (E_Segments) _f_Owner[Entity.Index];
		}
		set
		{
			_f_Owner[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5270256987534184595UL,7271033944239394211UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldString __f_ObjectTitle;
	public static BansheeGz.BGDatabase.BGFieldString _f_ObjectTitle
	{
		get
		{
			if(__f_ObjectTitle==null || __f_ObjectTitle.IsDeleted) __f_ObjectTitle=(BansheeGz.BGDatabase.BGFieldString) MetaDefault.GetField(new BGId(5394372170959802640UL,9009507287611560632UL));
			return __f_ObjectTitle;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldString __f_ObjectSubText;
	public static BansheeGz.BGDatabase.BGFieldString _f_ObjectSubText
	{
		get
		{
			if(__f_ObjectSubText==null || __f_ObjectSubText.IsDeleted) __f_ObjectSubText=(BansheeGz.BGDatabase.BGFieldString) MetaDefault.GetField(new BGId(5594065245607900864UL,6912274191189485987UL));
			return __f_ObjectSubText;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldUnityPrefab __f_Prefab;
	public static BansheeGz.BGDatabase.BGFieldUnityPrefab _f_Prefab
	{
		get
		{
			if(__f_Prefab==null || __f_Prefab.IsDeleted) __f_Prefab=(BansheeGz.BGDatabase.BGFieldUnityPrefab) MetaDefault.GetField(new BGId(4702383594286821901UL,9980623028393390773UL));
			return __f_Prefab;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Owner;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Owner
	{
		get
		{
			if(__f_Owner==null || __f_Owner.IsDeleted) __f_Owner=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(5020747016533988793UL,12419271386286385327UL));
			return __f_Owner;
		}
	}
	private static readonly List<BGEntity> _tugjbyuhfv_reusableList = new List<BGEntity>();
	public List<E_ObjectsGameplayData> RelatedObjectsGameplayDataListUsingObjectRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_ObjectsGameplayData._f_Object.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_ObjectsGameplayData>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_ObjectsGameplayData) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
}
