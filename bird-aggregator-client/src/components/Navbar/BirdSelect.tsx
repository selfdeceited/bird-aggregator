import * as Blueprint from '@blueprintjs/core'
import * as React from 'react'

import { ItemPredicate, ItemRenderer, Select } from '@blueprintjs/labs'

import { Bird } from '../../clients/GalleryClient'

export const BirdSelect = Select.ofType<Bird>()

export const filterBird: ItemPredicate<Bird> = (query: string, bird: Bird) => (
	`${bird.name.toLowerCase()} ${bird.latin.toLowerCase()}`.includes(
		query.toLowerCase(),
	)
)

export const renderBird: ItemRenderer<Bird> = (
	bird: Bird,
	{ handleClick },
) => (
	<Blueprint.MenuItem
		className=""
		key={bird.name}
		label={bird.latin}
		onMouseDown={handleClick}
		text={bird.name}
	/>
)
