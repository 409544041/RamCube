using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using Alias_rifegrt_LevelData = E_LevelData;
using Alias_rifegrt_LevelGameplayData = E_LevelGameplayData;
using Alias_rifegrt_MapWalls = E_MapWalls;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_Pin")]
public partial class M_Pin : BGEntityGo
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
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(4954953875072214365UL,15478807282525472406UL));
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
	public E_Biome f_Biome
	{
		get
		{
			return (E_Biome) _f_Biome[Entity.Index];
		}
		set
		{
			_f_Biome[Entity.Index] = value;
		}
	}
	public System.Int32 f_Index
	{
		get
		{
			return _f_Index[Entity.Index];
		}
		set
		{
			_f_Index[Entity.Index] = value;
		}
	}
	public System.Int32 f_PinTextUI
	{
		get
		{
			return _f_PinTextUI[Entity.Index];
		}
		set
		{
			_f_PinTextUI[Entity.Index] = value;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5700619351467592780UL,9596716367873031603UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Biome;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Biome
	{
		get
		{
			if(__f_Biome==null || __f_Biome.IsDeleted) __f_Biome=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(5355639157912867324UL,1763305154346170539UL));
			return __f_Biome;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldInt __f_Index;
	public static BansheeGz.BGDatabase.BGFieldInt _f_Index
	{
		get
		{
			if(__f_Index==null || __f_Index.IsDeleted) __f_Index=(BansheeGz.BGDatabase.BGFieldInt) MetaDefault.GetField(new BGId(5713392823763091186UL,10906278620831617424UL));
			return __f_Index;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldInt __f_PinTextUI;
	public static BansheeGz.BGDatabase.BGFieldInt _f_PinTextUI
	{
		get
		{
			if(__f_PinTextUI==null || __f_PinTextUI.IsDeleted) __f_PinTextUI=(BansheeGz.BGDatabase.BGFieldInt) MetaDefault.GetField(new BGId(5208647534428573803UL,268406907975443346UL));
			return __f_PinTextUI;
		}
	}
	private static readonly List<BGEntity> _tugjbyuhfv_reusableList = new List<BGEntity>();
	public List<E_LevelData> RelatedLevelDataListUsingPinRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_LevelData._f_Pin.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_LevelData>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_LevelData) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
	public List<E_LevelData> RelatedLevelDataListUsingUnlocksPinsRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_LevelData._f_UnlocksPins.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_LevelData>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_LevelData) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
	public List<E_LevelGameplayData> RelatedLevelGameplayDataListUsingPinRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_LevelGameplayData._f_Pin.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_LevelGameplayData>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_LevelGameplayData) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
	public List<E_MapWalls> RelatedMapWallsListUsingOriginPinRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_MapWalls._f_OriginPin.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_MapWalls>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_MapWalls) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
	public List<E_MapWalls> RelatedMapWallsListUsingDestPinRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_MapWalls._f_DestPin.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_MapWalls>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_MapWalls) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
}
