/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable no-shadow */
/* eslint-disable @typescript-eslint/no-unsafe-return */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-unsafe-call */
import * as React from 'react'

import { Callback, ComponentProps, PluginProps, addToolbarButton, createModule, makeUseContext } from 'yet-another-react-lightbox'
import { LightboxFooter, LightboxHeader } from './LightboxHeader'
import { useMemo, useState } from 'react'
import { ImagePropsGetter } from './GalleryLightbox'


interface CaptionsRef {
	/** if `true`, captions are visible */
	visible: boolean
	/** show captions */
	show: Callback
	/** hide captions */
	hide: Callback
}

type Props = Pick<PluginProps, 'augment' | 'addModule'> & {
	getImageProps: ImagePropsGetter
}

const PLUGIN_CAPTIONS_CUSTOM = 'captions_custom'
/** Captions plugin */
export function CaptionsCustom({ augment, addModule, getImageProps }: Props): void {
	augment(
		({ render: { slideFooter: renderFooter, ...restRender }, toolbar, ...restProps }) => ({
			render: {
				slideFooter: ({ slide }) => {
					const { header, description } = getImageProps(slide)
					return (
						<>
							{renderFooter?.({ slide })}

							{<LightboxHeader
								birdIds={header.birdIds}
								birdNames={header.birdNames}
								dateTaken={header.dateTaken}
								hostingLink={header.hostingLink} />}

							{<LightboxFooter text={description} />}
						</>
					)
				},
				...restRender,
			},
			toolbar: addToolbarButton(toolbar, PLUGIN_CAPTIONS_CUSTOM, null),
			...restProps,
		})
	)

	addModule(createModule(PLUGIN_CAPTIONS_CUSTOM, CaptionsContextProvider))
}


export const CustomCaptionsContext = React.createContext<CaptionsRef | null>(null)

export const useCaptions = makeUseContext('useCaptions', 'CustomCaptionsContext', CustomCaptionsContext)

// eslint-disable-next-line @typescript-eslint/explicit-function-return-type
export function CaptionsContextProvider({ children }: ComponentProps) {
	const [visible, setVisible] = useState(true)

	const context = useMemo(
		() => ({
			visible,
			show: () => setVisible(true),
			hide: () => setVisible(false),
		}),
		[visible]
	)
	return <CustomCaptionsContext.Provider value={context}>{children}</CustomCaptionsContext.Provider>
}
