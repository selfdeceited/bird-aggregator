import { NewWindowLinkStyled } from "./NewWindowLinkStyled"
import React from "react"

export const WikiDescription: React.FC<{name: string, wikiInfo: string}> = ({name, wikiInfo}) => {
    return (
    <div>
        <h2>{name}</h2>
        <div dangerouslySetInnerHTML={{ __html: wikiInfo }}></div>
        <NewWindowLinkStyled
            href={'https://en.wikipedia.org/wiki/' + name}
            target="_blank">more from Wikipedia...</NewWindowLinkStyled>
    </div>)
}