﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace KERBALISM {


public class KerbalData
{
  public KerbalData()
  {
    rescue     = true;
    disabled   = false;
    eva_dead   = false;
    rules      = new Dictionary<string, RuleData>();
  }

  public KerbalData(ConfigNode node)
  {
    rescue     = Lib.ConfigValue(node, "rescue", Lib.ConfigValue(node, "resque", true)); //< support pre 1.1.9 typo
    disabled   = Lib.ConfigValue(node, "disabled", false);
    eva_dead   = Lib.ConfigValue(node, "eva_dead", false);
    rules      = new Dictionary<string, RuleData>();

    foreach(var rule_node in node.GetNode("rules").GetNodes())
    {
      rules.Add(DB.from_safe_key(rule_node.name), new RuleData(rule_node));
    }
  }

  public void save(ConfigNode node)
  {
    node.AddValue("rescue", rescue);
    node.AddValue("disabled", disabled);
    node.AddValue("eva_dead", eva_dead);
    var rules_node = node.AddNode("rules");

    foreach(var p in rules)
    {
      p.Value.save(rules_node.AddNode(DB.to_safe_key(p.Key)));
    }
  }

  public RuleData Rule(string name)
  {
    if (!rules.ContainsKey(name))
    {
      rules.Add(name, new RuleData());
    }
    return rules[name];
  }

  public bool rescue;         // used to deal with rescue mission kerbals
  public bool disabled;       // a generic flag to disable resource consumption, for use by other mods
  public bool eva_dead;       // the eva kerbal died, and is now a floating body
  public Dictionary<string, RuleData> rules; // rules data
}


} // KERBALISM

