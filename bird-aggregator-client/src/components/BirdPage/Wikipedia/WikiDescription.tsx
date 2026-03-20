/* eslint-disable react/no-danger, @typescript-eslint/naming-convention, react/jsx-no-bind*/

import * as Collapsible from '@radix-ui/react-collapsible'
import React, { useState } from 'react'
import styled from 'styled-components'

import { NewWindowLinkStyled } from './NewWindowLinkStyled'

const ToggleButton = styled.button`
  display: inline-flex;
  align-items: center;
  padding: 5px 12px;
  border-radius: 3px;
  border: 1px solid #d0d0d0;
  background: transparent;
  font-size: 14px;
  cursor: pointer;
  &:hover { background: #f0f4f8; }
`

const WikiContent = styled.div`
  margin-top: 10px;
`

export const WikiDescription: React.FC<{ birdName: string; wikiInfo: string }> = ({ birdName, wikiInfo }) => {
	const [open, setOpen] = useState(false)

	return (
		<div>
			<h2>{birdName}</h2>
			<Collapsible.Root open={open} onOpenChange={setOpen}>
				<Collapsible.Trigger asChild>
					<ToggleButton>{open ? 'Hide' : 'Show'} wiki info</ToggleButton>
				</Collapsible.Trigger>
				<Collapsible.Content>
					<WikiContent dangerouslySetInnerHTML={{ __html: wikiInfo }}/>
					<NewWindowLinkStyled href={`https://en.wikipedia.org/wiki/${birdName}`} target="_blank">
						more from Wikipedia...
					</NewWindowLinkStyled>
				</Collapsible.Content>
			</Collapsible.Root>
		</div>
	)
}
