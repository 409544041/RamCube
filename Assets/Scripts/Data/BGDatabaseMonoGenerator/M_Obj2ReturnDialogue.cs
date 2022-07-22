using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_Obj2ReturnDialogue")]
public partial class M_Obj2ReturnDialogue : BGEntityGo
{
	public override BGMetaEntity MetaConstraint => MetaDefault;
	private static BansheeGz.BGDatabase.BGMetaNested _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaNested MetaDefault => _metaDefault ?? (_metaDefault = BGCodeGenUtils.GetMeta<BansheeGz.BGDatabase.BGMetaNested>(new BGId(5496949467946045557UL,12717756164536018082UL), () => _metaDefault = null));
	public static BansheeGz.BGDatabase.BGRepoEvents Events => BGRepo.I.Events;
	public System.String f_name
	{
		get => _f_name[Entity.Index];
		set => _f_name[Entity.Index] = value;
	}
	public E_ReturnDialogues f_ReturnDialogues
	{
		get => (E_ReturnDialogues) _f_ReturnDialogues[Entity.Index];
		set => _f_ReturnDialogues[Entity.Index] = value;
	}
	public System.Int32 f_CharIndex
	{
		get => _f_CharIndex[Entity.Index];
		set => _f_CharIndex[Entity.Index] = value;
	}
	public Expressions f_Expression
	{
		get => (Expressions) _f_Expression[Entity.Index];
		set => _f_Expression[Entity.Index] = value;
	}
	public System.String f_LocalizedText
	{
		get => _f_LocalizedText[Entity.Index];
		set => _f_LocalizedText[Entity.Index] = value;
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName _ufle12jhs77_f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name => _ufle12jhs77_f_name ?? (_ufle12jhs77_f_name = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEntityName>(MetaDefault, new BGId(5292865923303499052UL, 12683756453532068008UL), () => _ufle12jhs77_f_name = null));
	private static BansheeGz.BGDatabase.BGFieldRelationSingle _ufle12jhs77_f_ReturnDialogues;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_ReturnDialogues => _ufle12jhs77_f_ReturnDialogues ?? (_ufle12jhs77_f_ReturnDialogues = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldRelationSingle>(MetaDefault, new BGId(5149469727122951529UL, 4612976653520176269UL), () => _ufle12jhs77_f_ReturnDialogues = null));
	private static BansheeGz.BGDatabase.BGFieldInt _ufle12jhs77_f_CharIndex;
	public static BansheeGz.BGDatabase.BGFieldInt _f_CharIndex => _ufle12jhs77_f_CharIndex ?? (_ufle12jhs77_f_CharIndex = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldInt>(MetaDefault, new BGId(4996622358886042851UL, 16317126065324768701UL), () => _ufle12jhs77_f_CharIndex = null));
	private static BansheeGz.BGDatabase.BGFieldEnum _ufle12jhs77_f_Expression;
	public static BansheeGz.BGDatabase.BGFieldEnum _f_Expression => _ufle12jhs77_f_Expression ?? (_ufle12jhs77_f_Expression = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEnum>(MetaDefault, new BGId(4740454893205126632UL, 13891137720697003392UL), () => _ufle12jhs77_f_Expression = null));
	private static BansheeGz.BGDatabase.BGFieldLocalizedText _ufle12jhs77_f_LocalizedText;
	public static BansheeGz.BGDatabase.BGFieldLocalizedText _f_LocalizedText => _ufle12jhs77_f_LocalizedText ?? (_ufle12jhs77_f_LocalizedText = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldLocalizedText>(MetaDefault, new BGId(4891155471082277268UL, 3028504640822351769UL), () => _ufle12jhs77_f_LocalizedText = null));
}
