import 'dotenv/config';
import { Client } from '@notionhq/client';

const client = new Client({ auth: process.env.NOTION_TOKEN });
const databaseId = process.env.NOTION_DATABASE_ID as string;

type LogStruct = {
    level: string;
    message: string;
}

export async function WritieLog(message: string, type: string) {
    // try {
    //     const response = await client.pages.create({
    //         parent: {
    //             database_id: databaseId,
    //         },
    //         properties: {
    //             Type: {
    //                 rich_text: [
    //                     {
    //                         text: {
    //                             content: type
    //                         }
    //                     }
    //                 ]
    //             },
    //             Message: {
    //                 rich_text: [
    //                     {
    //                         text: {
    //                             content: message
    //                         }
    //                     }
    //                 ]
    //             }
    //         }
    //     });
    // } catch (error) {
    //     console.log(error);
    // }
}
