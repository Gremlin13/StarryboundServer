﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.avilance.Starrybound.Permissions;

namespace com.avilance.Starrybound.Commands
{
    class GroupCommand : CommandBase
    {
        public GroupCommand(ClientThread client)
        {
            this.name = "group";
            this.HelpText = ": Allows you to manage permission groups. Type /group help for full instructions.";
            
            this.Permission = new List<string>();
            this.Permission.Add("admin.managegroups");

            this.player = client.playerData;
            this.client = client;
        }

        public override bool doProcess(string[] args)
        {
            if (args.Length < 1)
            {
                this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                return false;
            }

            string command = args[0].Trim().ToLower();
            
            if (command == "list") // List all groups start
            {
                string groupList = "";
                foreach (Group group in StarryboundServer.groups.Values)
                {
                    groupList += group.name + ", ";
                }
                // TODO: Remove last comma
                this.client.sendChatMessage("^#5dc4f4;Group list: " + groupList.Substring(0, groupList.Length - 2));
                return true;
            } // List all groups end
            else if (command == "add") // Add new group start
            {
                if (args.Length <= 1)
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                string groupName = args[1].Trim();
                if (String.IsNullOrEmpty(groupName) || String.IsNullOrWhiteSpace(groupName))
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                Dictionary<string, bool> pPerms = new Dictionary<string, bool>();
                pPerms.Add("client.*", true);
                pPerms.Add("chat.*", true);
                Group newGroup = new Group(groupName, null, null, pPerms);
                StarryboundServer.groups.Add(newGroup.name, newGroup);
                this.client.sendCommandMessage("New group " + newGroup.name + " has been added.");
                return true;
            } // Add new group end
            else if (command == "del")
            { // Delete a group start
                if (args.Length <= 1)
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                string groupName = args[1].Trim();
                if (String.IsNullOrEmpty(groupName) || String.IsNullOrWhiteSpace(groupName))
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                if (StarryboundServer.groups.ContainsKey(groupName))
                {
                    StarryboundServer.groups.Remove(groupName);
                    this.client.sendCommandMessage("Group " + groupName + " has been removed.");
                    return true;
                }
                else
                {
                    this.client.sendCommandMessage("A Group with the name " + groupName + " does not exist.");
                    return false;
                }
            } // Delete a group end
            else if (command == "info")
            { // Group info start
                if (args.Length <= 1)
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                string groupName = args[1].Trim();
                if (String.IsNullOrEmpty(groupName) || String.IsNullOrWhiteSpace(groupName))
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                if (StarryboundServer.groups.ContainsKey(groupName))
                {
                    Group targetGroup = StarryboundServer.groups[groupName];
                    this.client.sendCommandMessage("Group info: Name: " + targetGroup.name + "; Prefix: " + (String.IsNullOrEmpty(targetGroup.prefix) ? "None" : targetGroup.prefix) + "; Name color: " + (String.IsNullOrEmpty(targetGroup.nameColor) ? "None" : targetGroup.nameColor));
                    return true;
                }
                else
                {
                    this.client.sendCommandMessage("A Group with the name " + groupName + " does not exist.");
                    return false;
                }
            } // Group info end
            else if (command == "mod")
            { // Group mod start
                if (args.Length <= 3)
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                string groupName = args[1].Trim();
                string modCommand = args[2].Trim().ToLower();
                string modValue = args[3].Trim();

                if (String.IsNullOrEmpty(groupName) || String.IsNullOrEmpty(modCommand) || String.IsNullOrEmpty(modValue))
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                if (StarryboundServer.groups.ContainsKey(groupName))
                {
                    Group targetGroup = StarryboundServer.groups[groupName];
                    if (modCommand == "prefix")
                    {
                        targetGroup.prefix = modValue;
                        this.client.sendCommandMessage("Group " + targetGroup.name + "'s prefix is now set to " + modValue);
                        return true;
                    }
                    else if (modCommand == "color")
                    {
                        if (modValue.Length == 7 && modValue[0] == '#')
                        {
                            targetGroup.nameColor = modValue;
                            this.client.sendCommandMessage("Group " + targetGroup.name + "'s name color is now set to " + modValue);
                            return true;
                        }
                        else
                        {
                            this.client.sendCommandMessage("Invalid color. Must be a hex color starting with #");
                            return false;
                        }
                    }
                    else
                    {
                        this.client.sendCommandMessage("Invalid modification parameter. Use /group help for instructions.");
                        return false;
                    }
                }
                else
                {
                    this.client.sendCommandMessage("A Group with the name " + groupName + " does not exist.");
                    return false;
                }
            } // Group mod end
            else if (command == "permissions")
            { // Group permissions start
                if (args.Length <= 2)
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                string groupName = args[1].Trim();
                string prmCommand = args[2].Trim().ToLower();

                if (String.IsNullOrEmpty(groupName) || String.IsNullOrEmpty(prmCommand))
                {
                    this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                    return false;
                }

                if (StarryboundServer.groups.ContainsKey(groupName))
                {
                    Group targetGroup = StarryboundServer.groups[groupName];

                    if (prmCommand == "list")
                    {
                        string permissionList = "";
                        foreach (string prm in targetGroup.permissions.Keys)
                        {
                            permissionList += prm + "; ";
                        }
                        this.client.sendChatMessage("^#5dc4f4;Group permissions: " + permissionList.Substring(0, permissionList.Length -2));
                        return true;
                    }
                    else if (prmCommand == "add")
                    {
                        if (args.Length <= 3)
                        {
                            this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                            return false;
                        }

                        string prmValue = args[3].Trim();
                        if (targetGroup.givePermission(prmValue))
                        {
                            this.client.sendCommandMessage("Permission " + prmValue + " was successfully added.");
                            return true;
                        }
                        else
                        {
                            this.client.sendCommandMessage("Failed to add permission " + prmValue + ". Make sure it is valid.");
                            return false;
                        }
                    }
                    else if (prmCommand == "del")
                    {
                        if (args.Length <= 3)
                        {
                            this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                            return false;
                        }

                        string prmValue = args[3].Trim();
                        if (targetGroup.permissions.ContainsKey(prmValue))
                        {
                            targetGroup.permissions.Remove(prmValue);
                            this.client.sendCommandMessage("Permission " + prmValue + " was successfully removed.");
                            return true;
                        }
                        else
                        {
                            this.client.sendCommandMessage("Failed to remove permission " + prmValue + ". Make sure the group has it.");
                            return false;
                        }
                    }
                    else
                    {
                        this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                        return false;
                    }
                }
                else
                {
                    this.client.sendCommandMessage("A Group with the name " + groupName + " does not exist.");
                    return false;
                }
            }
            else if (command == "help")
            {
                this.client.sendChatMessage("^#5dc4f4;Group command help:");
                this.client.sendChatMessage("^#5dc4f4;/group list - shows a list of all groups.");
                this.client.sendChatMessage("^#5dc4f4;/group add <group name> - adds a new group.");
                this.client.sendChatMessage("^#5dc4f4;/group del <group name> - deletes a group.");
                this.client.sendChatMessage("^#5dc4f4;/group info <group name> - shows information about a group.");
                this.client.sendChatMessage("^#5dc4f4;/group mod <group name> <prefix/color> <value> - changes a group parameter.");
                this.client.sendChatMessage("^#5dc4f4;/group permissions list - lists the permissions of a group.");
                this.client.sendChatMessage("^#5dc4f4;/group permissions <add/del> <permission> - adds or removes a permission from the group.");
                return true;
            }
            else
            {
                this.client.sendCommandMessage("Invalid syntax. Use /group help for instructions.");
                return false;
            }
        }
    }
}
