using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.LowLevel;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

namespace Menu.Main
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {

        }

        private void Update()
        {
            if (Console.IsVisible())
            {
                if (Input.GetKeyDown(KeyCode.Insert))
                {
                    string text = Console.instance.m_input.text;
                    Console.instance.Print(text);
                    string[] array = text.Split(new char[]
                    {
                        '.'
                    });
                    ConsoleMenu.ConsoleMenu.ParseCommand(array);
                    Console.instance.m_input.text = string.Empty;
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete))
                Loader.Unload();
        }

        private void OnGUI()
        {
            var cam = Utils.GetMainCamera();

            if (!cam)
                return;

            Player.m_localPlayer.m_runSpeed = ConsoleMenu.ConsoleMenu._newSpeed;

            Player.m_localPlayer.m_maxCarryWeight = 69696;
            Player.m_localPlayer.SetMaxHealth(300, false);
            Player.m_localPlayer.SetHealth(300);
            Player.m_localPlayer.m_blockStaminaDrain = 0;
            Player.m_localPlayer.m_runStaminaDrain   = 0;
            Player.m_localPlayer.m_jumpStaminaUsage  = 0;
            Player.m_localPlayer.m_sneakStaminaDrain = 0;

            var characters = Character.GetAllCharacters();
            foreach (Character character in characters)
            {
                var position = character.transform.position;
                var screen   = cam.WorldToScreenPoint(position);
                var faction  = character.m_faction;

                var color = Color.yellow;
                
                if (faction == Character.Faction.Players)
                {
                    if (Player.m_localPlayer == character)
                        continue;

                    color = Color.blue;
                }
                else if (character.GetBaseAI().IsAlerted())
                {
                    color = Color.red;
                }
                
                if (screen.x < 0f || screen.x > (float)Screen.width || screen.y < 0f || screen.y > (float)Screen.height || screen.z > 0f)
                {
                    var distance = (int)(Vector2.Distance(Player.m_localPlayer.transform.position, character.transform.position));

                    screen.y = Screen.height - screen.y;

                    Render.Color = color;
                    Render.DrawString(new Vector2(screen.x, screen.y), character.m_name.Replace("$enemy_", ""));

                    Render.Color = Color.white;
                    Render.DrawString(new Vector2(screen.x, (screen.y) + 13), "[" + distance.ToString() + "m]");

                    var width     = 25;
                    var health    = character.GetHealth();
                    var maxHealth = character.GetMaxHealth();

                    screen.y += 23;

                    Render.Color = new Color(0, 0, 0);
                    Render.DrawBoxFill(new Vector2(screen.x - width / 2, screen.y + 1), new Vector2(width, 3), Render.Color);

                    Render.Color = new Color(0, 1, 0);
                    var health_width = new Vector2(health * width / maxHealth, 3);
                    Render.DrawBoxFill(new Vector2(screen.x - width / 2, screen.y + 1), health_width, Render.Color);

                    Render.Color = new Color(0, 0, 0);
                    Render.DrawBox(new Vector2(screen.x - width / 2, screen.y + 1), new Vector2(width, 3), 1, false);
                }
            }

            Vector3 vector = Player.m_localPlayer.transform.position + Vector3.up;
            foreach (Collider collider in Physics.OverlapSphere(vector, 100f, LayerMask.GetMask("item")))
            {
                if (collider.attachedRigidbody)
                {
                    ItemDrop component = collider.attachedRigidbody.GetComponent<ItemDrop>();
                    if (!(component == null) && component.GetComponent<ZNetView>().IsValid())
                    {
                        var position = component.gameObject.transform.position;
                        var screen = cam.WorldToScreenPoint(position);

                        if (screen.x < 0f || screen.x > (float)Screen.width || screen.y < 0f || screen.y > (float)Screen.height || screen.z > 0f)
                        {
                            var distance = (int)(Vector2.Distance(Player.m_localPlayer.transform.position, position));

                            Render.Color = Color.gray;
                            Render.DrawString(new Vector2(screen.x, Screen.height - screen.y), component.name.Replace("(Clone)", ""));

                            Render.Color = Color.white;
                            Render.DrawString(new Vector2(screen.x, (Screen.height - screen.y) + 13), "[" + distance.ToString() + "m]");
                        }
                    }
                }
            }
        }
    }
}
