import * as React from 'react'
import * as axios from '../http.adapter'

import { BirdImage, Image } from './BirdImage'

import { BirdLightbox } from './BirdLightbox'
import Gallery from 'react-photo-gallery'
import { Link } from 'react-router-dom'
import Measure from 'react-measure'

export interface IGalleryProps {
	seeFullGalleryLink: boolean
	urlToFetch: string
}

interface IGalleryState {
	images: Image[]
	fullGallery: boolean
	width: number
	selectedIndex: number | null
}

export class GalleryWrap extends React.Component<IGalleryProps, IGalleryState> {
	constructor(props: IGalleryProps) {
		super(props)

		this.state = {
			fullGallery: !props.seeFullGalleryLink,
			images: [],
			selectedIndex: null,
			width: -1,
		}

		this.onMouseDown = this.onMouseDown.bind(this)
	}

	public componentDidMount() {
		this.fetchTheUrl(this.props.urlToFetch)
	}

	public componentWillReceiveProps(nextProps: any) {
		this.fetchTheUrl(nextProps.urlToFetch)
		this.setState({ selectedIndex: null })
	}

	public onLightBoxClose() {
		this.setState({
			selectedIndex: null,
		})
	}

	public render() {
		const latestShotsLink = this.state.fullGallery ? (
			<div></div>
		) : (
			<div>
				<h2 className="latest-shots">Latest photos</h2>
				<Link to="/gallery" role="button" className="bp3-button bp3-minimal bp3-icon-camera small-margin">
					Check Out Full Gallery
				</Link>
			</div>
		)

		return (
			<Measure
				bounds
				onResize={contentRect =>
					this.setState({ width: (contentRect.bounds && contentRect.bounds.width) || 100 })
				}
			>
				{({ measureRef }) => {
					if (this.state.width < 1) {
						return <div ref={measureRef}></div>
					}

					const imageRenderer = ({ index, left, top, key, photo }: any) => (
						<BirdImage
							key={key}
							margin={'2px'}
							index={index}
							photo={photo}
							left={left}
							top={top}
							onMouseDown={this.onMouseDown}
						/>
					)

					return (
						<div ref={measureRef}>
							<div className="body">
								{latestShotsLink}
								<Gallery
									photos={this.state.images}
									columns={this.getColumnSize(this.state.width)}
									renderImage={imageRenderer}
									onClick={this.onMouseDown}
								/>
							</div>
							{this.state.selectedIndex !== null ? (
								<BirdLightbox
									photos={this.state.images}
									index={this.state.selectedIndex}
									onClose={() => this.onLightBoxClose()}
								/>
							) : null}
						</div>
					)
				}}
			</Measure>
		)
	}

	private fetchTheUrl(url: string) {
		axios.get(url).then(res => {
			const images = res.data.photos.map((x: Image) => {
				x.tags = [{ title: x.caption, value: x.caption }]
				return x
			})
			this.setState({ images })
		})
	}

	private getColumnSize(width: number) {
		const columnWidthMap = [
			{ cols: 1, minWidth: 1 },
			{ cols: 2, minWidth: 480 },
			{ cols: 3, minWidth: 1024 },
			{ cols: 4, minWidth: 1824 },
		]

		let columns = 0
		columnWidthMap.forEach(x => (columns = width >= x.minWidth ? x.cols : columns))
		return columns
	}

	private onMouseDown(event: any, obj: any) {
		this.setState({ selectedIndex: obj.index })
	}
}
