# MacroNewt FAQ
Frequently asked questions about MacroNewt

## What is MacroNewt?
If the [Introduction](~/articles/intro.md) page didn't answer your question, I'll try again. *MacroNewt* is a web application built to provide an easy way to record and log the foods a person eats throughout the day. By utilizing the USDA's Food and Nutrition database, *MacroNewt* provides the user with the nutritional content of the foods he or she eats. There is a particular focus on the macronutrient (e.g. fat, protein, carbohydrates) content of the food, thus the name, and thus the ability for the user to adjust daily goals based on a tailored macronutrient balance.

## Why was MacroNewt created?
It was built solely as a learning project. Having learned very little about web applications throughout my time earning a BS in Computer Science, and believing it to be extremely relevant knowledge for someone trying to enter the industry, I decided to take the time to teach myself. Having an interest in healthy eating led me to the idea of utilizing the USDA Food and Nutrition database as the building block for my web app. Once I had a basic idea of what I wanted MacroNewt to be, I took on the task of building it while being sure to make it complex *enough* to provide a challenge and encourage a worthwhile learning experience. 

## How does MacroNewt know about foods?
MacroNewt retrieves all food and nutritional information from the USDA's open source database. While there is both an unbranded and branded data source, the strength of the database lies in individual food components rather than commonly available commercial foods (e.g. popular fast food items or restaurant foods). It should be noted that during the time MacroNewt was being built, the USDA announced a migration to a new database system to be fully implemented by spring of 2020. As of the beginning of 2020 the old API calls used in the application still work, but will need adjustments in the near future. 

## Should I trust MacroNewt with my health?
No. The information, including but not limited to, text, graphics, images and other material contained on this website are for informational purposes only. MacroNewt is meant to provide simple help in focusing on and tracking dietary trends, but it is not supposed to take the place of any professional/medical directions or advice.