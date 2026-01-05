using Assets._Scripts.Core.Artifacts.StatModifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace MagmaHeart.Core.Artifacts
{
    public class ArtifactDataDeserializer
    {
        private const string ARTIFACT_XPATH = "/Artifact";

        public ArtifactData Deserialize(TextAsset artifactDataFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(new StringReader(artifactDataFile.text));

            string name = document.SelectSingleNode($"{ARTIFACT_XPATH}/Name").InnerText;
            string description = document.SelectSingleNode($"{ARTIFACT_XPATH}/Description").InnerText;

            string rawRarity = document.SelectSingleNode($"{ARTIFACT_XPATH}/Rarity").InnerText;
            Rarity rarity = Enum.Parse<Rarity>(rawRarity);

            string iconPath = document.SelectSingleNode($"{ARTIFACT_XPATH}/IconResourcePath").InnerText;
            Sprite icon = ExternalResources.LoadSprite(iconPath);

            List<List<IStatModifier>> modifiers = new List<List<IStatModifier>>();
            XmlNodeList levelNodes = document.SelectNodes($"{ARTIFACT_XPATH}/Level");
            
            for (int i = 0; i < levelNodes.Count; i++)
            {
                modifiers.Add(new List<IStatModifier>());
                XmlNode levelNode = levelNodes[i];

                XmlNodeList rawModifiers = levelNode.SelectSingleNode("StatModifiers").ChildNodes;
                foreach (XmlNode modifierNode in rawModifiers)
                {
                    IStatModifier modifier = DeserializeStatModifier(modifierNode);
                    modifiers[i].Add(modifier);
                }
            }

            return new ArtifactData(name, description, rarity, icon, modifiers);
        }

        private IStatModifier DeserializeStatModifier(XmlNode statModifierNode)
        {
            switch (statModifierNode.Name)
            {
                case "HealthModifier":
                    int additionalHealth = Int32.Parse(statModifierNode.Attributes["AdditionalHealth"].Value);
                    return new HealthStatModifier(additionalHealth);
                default:
                    throw new XmlException($"Unknown StatModifier type: {statModifierNode.Name}");
            }
        }
    }
}
