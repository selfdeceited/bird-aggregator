/* eslint-disable react/no-danger, @typescript-eslint/naming-convention*/

import { NewWindowLinkStyled } from './NewWindowLinkStyled'
import React from 'react'

export const WikiDescription: React.FC<{birdName: string; wikiInfo: string}> = ({ birdName, wikiInfo }) => (
	<div>
		<h2>{birdName}</h2>
		<div dangerouslySetInnerHTML={{ __html: wikiInfo }}></div>
		<NewWindowLinkStyled
			href={`https://en.wikipedia.org/wiki/${birdName}`}
			target="_blank">more from Wikipedia...</NewWindowLinkStyled>
	</div>)
