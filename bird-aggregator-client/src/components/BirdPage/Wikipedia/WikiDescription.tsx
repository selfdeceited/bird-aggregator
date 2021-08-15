/* eslint-disable react/no-danger, @typescript-eslint/naming-convention, react/jsx-no-bind*/

import { Button, Collapse } from '@blueprintjs/core'
import React, { useState } from 'react'

import { NewWindowLinkStyled } from './NewWindowLinkStyled'

export const WikiDescription: React.FC<{ birdName: string; wikiInfo: string }> = ({ birdName, wikiInfo }) => {
	const [showWikiInfo, setShowWikiInfo] = useState(false)

	return (
		<div>
			<h2>{birdName}</h2>
			<Button onMouseDown={() => setShowWikiInfo(!showWikiInfo)}>{showWikiInfo ? 'Hide' : 'Show'} wiki info</Button>
			<Collapse isOpen={showWikiInfo}>
				<div
					style={{ marginTop: 10 }}
					dangerouslySetInnerHTML={{ __html: wikiInfo }}></div>
				<NewWindowLinkStyled href={`https://en.wikipedia.org/wiki/${birdName}`} target="_blank">
					more from Wikipedia...
				</NewWindowLinkStyled>
			</Collapse>
		</div>
	)
}
