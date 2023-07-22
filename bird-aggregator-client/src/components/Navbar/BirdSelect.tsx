
import * as React from 'react'
import { ItemPredicate, ItemRenderer } from '@blueprintjs/select'

import { Bird } from '../../clients/GalleryClient'
import { MenuItem } from '@blueprintjs/core'

export const filterBird: ItemPredicate<Bird> = (query: string, bird: Bird) => (
	`${bird.name.toLowerCase()} ${bird.latin.toLowerCase()}`.includes(
		query.toLowerCase(),
	)
)

export const renderBird: ItemRenderer<Bird> = (
	bird: Bird,
	{ handleClick },
) => (
	<MenuItem
		className=""
		key={bird.name}
		label={bird.latin}
		onMouseDown={handleClick}
		text={bird.name}
	/>
)
