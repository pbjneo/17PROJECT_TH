using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;


public class Atlas : MonoBehaviour
{
	[MenuItem("Atlas/1.Link[UISprite -> Dynamic_Atlas]")]
	static void ADD_Dynamic_Atlas()
	{
		//********* 1차 비교대상 수집(Atlas에 등록한 Texture File의 이름으로 비교)

		//리소스 폴더 파일 로드
		string[][] FineInfo = new string[3][];
		FineInfo[0] = Directory.GetFiles(Application.dataPath + "/[[ExportData]]/[ResourceAtlas]/CombineUI/Texture/");
		FineInfo[1] = Directory.GetFiles(Application.dataPath + "/[[ExportData]]/[ResourceAtlas]/LobbyUI/Texture/");
		FineInfo[2] = Directory.GetFiles(Application.dataPath + "/[[ExportData]]/[ResourceAtlas]/BattleUI/Texture/");

		//선택된 Hierarchy 게임오브젝트들 로드
		GameObject[] selection = Selection.gameObjects;
		for (int i = 0; i < selection.Length; i++)
		{
			Transform tr = selection[i].transform;

			//********* 2차 자식들 수집(UISprite컴포넌트들)

			//오브젝트의 모든 UISprite 호출(비활성화 포함)
			UISprite[] SpriteList = tr.GetComponentsInChildren<UISprite>(true);

			for (int j = 0; j < SpriteList.Length; j++)
			{
				//UISprite_Dynamic_Atlas 클래스 유무 확인 후 없을시 ADD
				UISprite_Dynamic_Atlas Dynamic_Atlas = SpriteList[j].GetComponent<UISprite_Dynamic_Atlas>();
				if (Dynamic_Atlas == null)
				{
					SpriteList[j].gameObject.AddComponent<UISprite_Dynamic_Atlas>();
					Dynamic_Atlas = SpriteList[j].GetComponent<UISprite_Dynamic_Atlas>();
				}


				//********* 3차 UISprite가 가지고 있는 spriteName 링크

				//SpriteName이 정상적일시
				if (false == string.IsNullOrEmpty(SpriteList[j].spriteName))
				{
					switch (SpriteList[j].spriteName)
					{
						#region 임시변환(리소스정리용)
						case "c1-1": Dynamic_Atlas.spriteName = "icon_character3"; break;
						case "vs_1": Dynamic_Atlas.spriteName = "effect_3"; break;
						case "stage_3_2": Dynamic_Atlas.spriteName = "back_2"; break;
						case "10_titile_15": Dynamic_Atlas.spriteName = "icon_ticket"; break;

						case "4_login_34": Dynamic_Atlas.spriteName = "back_4"; break;
						case "stage_2_6": Dynamic_Atlas.spriteName = "back_1"; break;
						case "0_play_01": Dynamic_Atlas.spriteName = "btn_2"; break;
						case "ADD4_2": Dynamic_Atlas.spriteName = "progress_2"; break;
						case "11_Skill_11":
						case "inven_3_7": Dynamic_Atlas.spriteName = "icon_arrow"; break;
						case "ADD1_11": Dynamic_Atlas.spriteName = "buff1-1"; break;
						case "0_main_21": Dynamic_Atlas.spriteName = "gambl_1_2"; break;
						case "item1091": Dynamic_Atlas.spriteName = "gambl_1_4"; break;
						case "item1092": Dynamic_Atlas.spriteName = "gambl_1_5"; break;
						case "13_character_13": Dynamic_Atlas.spriteName = "icon_job1"; break;
						case "ADD4-11": Dynamic_Atlas.spriteName = "game_1_6"; break;
						case "vip_3": Dynamic_Atlas.spriteName = "icon_vip3"; break;
						case "c1-2": Dynamic_Atlas.spriteName = "icon_character1"; break;
						case "ADD2_8": Dynamic_Atlas.spriteName = "back_2"; break;
						case "Login_3": Dynamic_Atlas.spriteName = "login_3"; break;
						case "3-12": Dynamic_Atlas.spriteName = "icon_exit"; break;
						case "1-9": Dynamic_Atlas.spriteName = "icon_new"; break;
						case "gambl_1_6": Dynamic_Atlas.spriteName = "back_4"; break;
						case "11_Skill_3":
						case "0_main_33":
						case "13_character_07": Dynamic_Atlas.spriteName = "btn_1"; break;
						case "5_shop_07": Dynamic_Atlas.spriteName = "icon_gold"; break;
						case "4_login_26": Dynamic_Atlas.spriteName = "icon_gem"; break;
						case "stage_3_1": Dynamic_Atlas.spriteName = "back_1"; break;
						case "3_inven_14":
						case "UI_Chat_BG": Dynamic_Atlas.spriteName = "fade"; SpriteList[j].alpha = 160f / 255f; break;
						case "UI_Gem2": Dynamic_Atlas.spriteName = "icon_gem"; SpriteList[j].type = UIBasicSprite.Type.Simple; SpriteList[j].width = 60; SpriteList[j].height = 50; break;
						#endregion
						default:
							Dynamic_Atlas.spriteName = SpriteList[j].spriteName;
							break;
					}
					//아틀라스 변경(지금은 리소스 폴더들을 찾아 이미지파일 이름과 맹칭이 됫을 시 해당하는 폴더의 아틀라스로 정하고 있다.
					//리소스 정리 후 그냥 기존의 아틀라스 명을 그대로 옮기게 수정하면 된다.


					//********* 4차 UISprite가 가지고 있는 Atlas 링크
					string name = Dynamic_Atlas.spriteName;
					AtlasMgr.AtlasKind atlasname = AtlasMgr.AtlasKind.End;
					for (int h = 0; h < 3; h++)
					{
						for (int k = 0; k < FineInfo[h].Length; k++)
						{
							string[] temp = FineInfo[h][k].Split('/');
							string[] data = temp[temp.Length - 1].Split('.');
							if (name == data[0])
							{
								switch (h)
								{
									case 0: atlasname = AtlasMgr.AtlasKind.CombineUI; break;
									case 1: atlasname = AtlasMgr.AtlasKind.LobbyUI; break;
									case 2: atlasname = AtlasMgr.AtlasKind.BattleUI; break;
								}
								break;
							}
						}
						if (atlasname != AtlasMgr.AtlasKind.End)
							break;
					}

					if (atlasname != AtlasMgr.AtlasKind.End)
					{
						//Debug.Log("2.성공 obj:" + atlasname);
						Dynamic_Atlas.atlasKind = atlasname;
						SpriteList[j].atlas = null;
					}
					else
					{
						if (SpriteList[j].atlas != null)
							Debug.LogError("2.실패 Obj:" + SpriteList[j].gameObject.name + "기존아틀라스:" + SpriteList[j].atlas.name);
					}
				}
				else
				{
					Debug.LogError("1.실패 obj:" + tr.name);
				}
			}
		}
	}

	[MenuItem("Atlas/2.Link[Dynamic_Atlas -> UISprite]")]
	static void ADD_UISprite()
	{
		//*********1차 Atlas 수집
		string Asset_DataPath = "/[[ExportData]]/Atlas/";
		Dictionary<GameObject, UIAtlas> List_Atlas = new Dictionary<GameObject, UIAtlas>();
		DirectoryInfo D_Info = new DirectoryInfo(Application.dataPath + Asset_DataPath);
		FileInfo[] F_Info = D_Info.GetFiles("*.prefab");
		foreach (FileInfo info in F_Info)
		{
			Object t = AssetDatabase.LoadMainAssetAtPath("Assets" + Asset_DataPath + info.Name);
			Debug.Log(t.name);
			GameObject go = Instantiate(t) as GameObject;
			if (go == null)
				continue;
			go.name = t.name;
			List_Atlas.Add(go, go.GetComponent<UIAtlas>());
		}

		//*********2차 Sprite Link
		//선택된 Hierarchy 게임오브젝트들 로드
		GameObject[] selection = Selection.gameObjects;
		for (int i = 0; i < selection.Length; i++)
		{
			Transform tr = selection[i].transform;
			//오브젝트의 모든 UISprite 호출(비활성화 포함)
			UISprite_Dynamic_Atlas[] Dynamic_AtlasList = tr.GetComponentsInChildren<UISprite_Dynamic_Atlas>(true);
			for (int j = 0; j < Dynamic_AtlasList.Length; j++)
			{
				//UISprite_Dynamic_Atlas 클래스 유무 확인 후 없을시 ADD
				UISprite Objectsprite = Dynamic_AtlasList[j].GetComponent<UISprite>();
				if (Objectsprite == null)
				{
					Debug.LogError("실패: sprite = null / " + Dynamic_AtlasList[j].gameObject.name);
				}
				UIAtlas atlas = null;
				foreach (KeyValuePair<GameObject, UIAtlas> o in List_Atlas)
				{
					if (o.Key.name != Dynamic_AtlasList[j].atlasKind.ToString())
						continue;
					atlas = o.Value;
				}
				if (atlas == null){
					Debug.LogError("실패: atlas = null / altas:" + Dynamic_AtlasList[j].atlasKind.ToString());
					continue;
				}
				Objectsprite.atlas = atlas;
				Objectsprite.spriteName = Dynamic_AtlasList[j].spriteName;
			}
		}
	}
}
