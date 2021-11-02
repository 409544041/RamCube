using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using Alias_rifegrt_Pin = E_Pin;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_Biome")]
public partial class M_Biome : BGEntityGo
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
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(5318121685371950766UL,2942031341061376408UL));
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
	public List<E_Variety> f_Variety
	{
		get
		{
			var val = _f_Variety[Entity.Index];
			if(val==null || val.Count==0) return null;
			var ___Variety = new List<E_Variety>();
			for (var i = 0; i < val.Count; i++) ___Variety.Add((E_Variety) val[i]);
			return ___Variety;
		}
	}
	public System.Single f_MinZ
	{
		get
		{
			return _f_MinZ[Entity.Index];
		}
		set
		{
			_f_MinZ[Entity.Index] = value;
		}
	}
	public System.Single f_MaxZ
	{
		get
		{
			return _f_MaxZ[Entity.Index];
		}
		set
		{
			_f_MaxZ[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(4811910356762317652UL,6316590992709792654UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldNested __f_Variety;
	public static BansheeGz.BGDatabase.BGFieldNested _f_Variety
	{
		get
		{
			if(__f_Variety==null || __f_Variety.IsDeleted) __f_Variety=(BansheeGz.BGDatabase.BGFieldNested) MetaDefault.GetField(new BGId(5722706716172130075UL,12926031450509768577UL));
			return __f_Variety;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldFloat __f_MinZ;
	public static BansheeGz.BGDatabase.BGFieldFloat _f_MinZ
	{
		get
		{
			if(__f_MinZ==null || __f_MinZ.IsDeleted) __f_MinZ=(BansheeGz.BGDatabase.BGFieldFloat) MetaDefault.GetField(new BGId(5374342133323149295UL,17281580155556473531UL));
			return __f_MinZ;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldFloat __f_MaxZ;
	public static BansheeGz.BGDatabase.BGFieldFloat _f_MaxZ
	{
		get
		{
			if(__f_MaxZ==null || __f_MaxZ.IsDeleted) __f_MaxZ=(BansheeGz.BGDatabase.BGFieldFloat) MetaDefault.GetField(new BGId(5628989294326945906UL,9389870997533305998UL));
			return __f_MaxZ;
		}
	}
	private static readonly List<BGEntity> _tugjbyuhfv_reusableList = new List<BGEntity>();
	public List<E_Pin> RelatedPinListUsingBiomeRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_Pin._f_Biome.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_Pin>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_Pin) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
}
